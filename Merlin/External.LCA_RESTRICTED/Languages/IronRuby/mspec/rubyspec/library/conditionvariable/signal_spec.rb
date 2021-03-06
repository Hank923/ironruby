require File.dirname(__FILE__) + '/../../spec_helper'
require 'thread'

describe "ConditionVariable#signal" do
  it "should return self if nothing to signal" do
    cv = ConditionVariable.new
    cv.signal.should == cv
  end

  it "should return self if something is waiting for a signal" do
    m = Mutex.new
    cv = ConditionVariable.new
    th = Thread.new do
      m.synchronize do
        cv.wait(m)
      end
    end

    # ensures that th grabs m before current thread
    Thread.pass until th.status == "sleep"

    m.synchronize { cv.signal }.should == cv

    th.join
  end

  it "releases the first thread waiting in line for this resource" do
    m = Mutex.new
    cv = ConditionVariable.new
    threads = []
    r1 = []
    r2 = []

    # large number to attempt to cause race conditions
    100.times do |i|
      threads << Thread.new(i) do |tid|
        m.synchronize do
          r1 << tid
          cv.wait(m)
          r2 << tid
        end
      end
    end

    # wait for all threads to acquire the mutex the first time
    Thread.pass until m.synchronize { r1.size == threads.size }
    # wait until all threads are sleeping (ie waiting)
    Thread.pass until threads.all? {|th| th.status == "sleep" }

    r2.should be_empty
    100.times do |i|
      m.synchronize do
        cv.signal
      end
      Thread.pass until r2.size == i+1
    end

    threads.each {|t| t.join }

    # ensure that all the threads that went into the cv.wait are
    # released in the same order
    r2.should == r1
  end
end

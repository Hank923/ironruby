require 'pathname'
require Pathname(__FILE__).dirname.expand_path.parent + 'spec_helper'

if HAS_SQLITE3 || HAS_MYSQL || HAS_POSTGRES
  describe DataMapper::Collection do
    it_should_behave_like 'It Has Setup Resources'

    before :all do
      @dragons   = Dragon.all
      @countries = Country.all
    end

    it_should_behave_like 'An Aggregatable Class'
  end
end

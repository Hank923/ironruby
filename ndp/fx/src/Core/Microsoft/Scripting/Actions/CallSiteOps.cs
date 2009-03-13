﻿/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. 
 *
 * This source code is subject to terms and conditions of the Microsoft Public License. A 
 * copy of the license can be found in the License.html file at the root of this distribution. If 
 * you cannot locate the  Microsoft Public License, please send an email to 
 * dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
 * by the terms of the Microsoft Public License.
 *
 * You must not remove this notice, or any other, from this software.
 *
 *
 * ***************************************************************************/

using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace System.Runtime.CompilerServices {

    // Conceptually these are instance methods on CallSite<T> but
    // we don't want users to see them

    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public static class CallSiteOps {

        /// <summary>
        /// Creates an instance of a dynamic call site used for cache lookup.
        /// </summary>
        /// <typeparam name="T">The type of the delegate of the <see cref="CallSite"/>.</typeparam>
        /// <returns>The new call site.</returns>
        [Obsolete("do not use this method", true), EditorBrowsable(EditorBrowsableState.Never)]
        public static CallSite<T> CreateMatchmaker<T>(CallSite<T> site) where T : class {
            var mm = site.CreateMatchMaker();
            CallSiteOps.ClearMatch(mm);
            return mm;
        }

        /// <summary>
        /// Checks if a dynamic site requires an update.
        /// </summary>
        /// <param name="site">An instance of the dynamic call site.</param>
        /// <returns>true if rule does not need updating, false otherwise.</returns>
        [Obsolete("do not use this method", true), EditorBrowsable(EditorBrowsableState.Never)]
        public static bool SetNotMatched(CallSite site) {
            var res = site._match;
            site._match = false;  //avoid branch here to make sure the method is inlined
            return res;
        }

        /// <summary>
        /// Checks whether the executed rule matched
        /// </summary>
        /// <param name="site">An instance of the dynamic call site.</param>
        /// <returns>true if rule matched, false otherwise.</returns>
        [Obsolete("do not use this method", true), EditorBrowsable(EditorBrowsableState.Never)]
        public static bool GetMatch(CallSite site) {
            return site._match;
        }

        /// <summary>
        /// Clears the match flag on the matchmaker call site.
        /// </summary>
        /// <param name="site">An instance of the dynamic call site.</param>
        [Obsolete("do not use this method", true), EditorBrowsable(EditorBrowsableState.Never)]
        public static void ClearMatch(CallSite site) {
            site._match = true;
        }

        /// <summary>
        /// Adds a rule to the cache maintained on the dynamic call site.
        /// </summary>
        /// <typeparam name="T">The type of the delegate of the <see cref="CallSite"/>.</typeparam>
        /// <param name="site">An instance of the dynamic call site.</param>
        /// <param name="rule">An instance of the call site rule.</param>
        [Obsolete("do not use this method", true), EditorBrowsable(EditorBrowsableState.Never)]
        public static void AddRule<T>(CallSite<T> site, T rule) where T : class {
            if (site.Rules == null) {
                site.Rules = new SmallRuleSet<T>(new[] { rule });
            } else {
                site.Rules = site.Rules.AddRule(rule);
            }
        }

        /// <summary>
        /// Updates rules in the cache.
        /// </summary>
        /// <typeparam name="T">The type of the delegate of the <see cref="CallSite"/>.</typeparam>
        /// <param name="this">An instance of the dynamic call site.</param>
        /// <param name="matched">The matched rule index.</param>
        [Obsolete("do not use this method", true), EditorBrowsable(EditorBrowsableState.Never)]
        public static void UpdateRules<T>(CallSite<T> @this, int matched) where T : class {
            if (matched > 1) {
                @this.Rules.MoveRule(matched);
            }
        }

        /// <summary>
        /// Gets the dynamic binding rules from the call site.
        /// </summary>
        /// <typeparam name="T">The type of the delegate of the <see cref="CallSite"/>.</typeparam>
        /// <param name="site">An instance of the dynamic call site.</param>
        /// <returns>An array of dynamic binding rules.</returns>
        [Obsolete("do not use this method", true), EditorBrowsable(EditorBrowsableState.Never)]
        public static T[] GetRules<T>(CallSite<T> site) where T : class {
            return (site.Rules == null) ? null : site.Rules.GetRules();
        }


        /// <summary>
        /// Retrieves binding rule cache.
        /// </summary>
        /// <typeparam name="T">The type of the delegate of the <see cref="CallSite"/>.</typeparam>
        /// <param name="site">An instance of the dynamic call site.</param>
        /// <returns>The cache.</returns>
        [Obsolete("do not use this method", true), EditorBrowsable(EditorBrowsableState.Never)]
        public static RuleCache<T> GetRuleCache<T>(CallSite<T> site) where T : class {
            return site.Binder.GetRuleCache<T>();
        }


        /// <summary>
        /// Moves the binding rule within the cache.
        /// </summary>
        /// <typeparam name="T">The type of the delegate of the <see cref="CallSite"/>.</typeparam>
        /// <param name="cache">The call site rule cache.</param>
        /// <param name="rule">An instance of the call site rule.</param>
        /// <param name="i">An index of the call site rule.</param>
        [Obsolete("do not use this method", true), EditorBrowsable(EditorBrowsableState.Never)]
        public static void MoveRule<T>(RuleCache<T> cache, T rule, int i) where T : class {
            if (i > 1) {
                cache.MoveRule(rule, i);
            }
        }

        /// <summary>
        /// Searches the dynamic rule cache for rules applicable to the dynamic operation.
        /// </summary>
        /// <typeparam name="T">The type of the delegate of the <see cref="CallSite"/>.</typeparam>
        /// <param name="cache">The cache.</param>
        /// <returns>The collection of applicable rules.</returns>
        [Obsolete("do not use this method", true), EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<T> GetCachedRules<T>(RuleCache<T> cache) where T : class {
            foreach (CallSiteRule<T> rule in cache.GetRules()) {
                yield return rule.Target;
            }
        }
    }
}

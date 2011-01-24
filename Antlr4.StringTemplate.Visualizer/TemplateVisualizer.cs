﻿/*
 * [The "BSD licence"]
 * Copyright (c) 2011 Terence Parr
 * All rights reserved.
 *
 * Conversion to C#:
 * Copyright (c) 2011 Sam Harwell, Tunnel Vision Laboratories, LLC
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

namespace Antlr4.StringTemplate.Visualizer
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Antlr4.StringTemplate.Debug;
    using Antlr4.StringTemplate.Misc;
    using ArgumentNullException = System.ArgumentNullException;

    public class TemplateVisualizer
    {
        private readonly ErrorManager _errorManager;
        private readonly DebugST _root;
        private readonly string _output;
        private readonly Interpreter _interpreter;
        private readonly List<string> _trace;
        private readonly ReadOnlyCollection<STMessage> _errors;

        public TemplateVisualizer(ErrorManager errorManager, DebugST root, string output, Interpreter interpreter, List<string> trace, ReadOnlyCollection<STMessage> errors)
        {
            if (errorManager == null)
                throw new ArgumentNullException("errorManager");
            if (root == null)
                throw new ArgumentNullException("root");
            if (interpreter == null)
                throw new ArgumentNullException("interpreter");
            if (trace == null)
                throw new ArgumentNullException("trace");
            if (errors == null)
                throw new ArgumentNullException("errors");

            _errorManager = errorManager;
            _root = root;
            _output = output;
            _interpreter = interpreter;
            _trace = trace;
            _errors = errors;
        }

        public ErrorManager ErrorManager
        {
            get
            {
                return _errorManager;
            }
        }

        public DebugST RootTemplate
        {
            get
            {
                return _root;
            }
        }

        public string Output
        {
            get
            {
                return _output;
            }
        }

        public Interpreter Interpreter
        {
            get
            {
                return _interpreter;
            }
        }

        public List<string> Trace
        {
            get
            {
                return _trace;
            }
        }

        public ReadOnlyCollection<STMessage> Errors
        {
            get
            {
                return _errors;
            }
        }

        public void Show()
        {
            TemplateVisualizerWindow window = new TemplateVisualizerWindow(this);
            window.ShowDialog();
        }
    }
}

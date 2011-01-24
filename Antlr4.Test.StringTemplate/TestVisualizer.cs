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

namespace Antlr4.Test.StringTemplate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Antlr4.StringTemplate;
    using Antlr4.StringTemplate.Debug;
    using CultureInfo = System.Globalization.CultureInfo;
    using Antlr4.StringTemplate.Misc;
    using StringWriter = System.IO.StringWriter;
    using Antlr4.StringTemplate.Visualizer;
    using Path = System.IO.Path;

    [TestClass]
    public class TestVisualizer : BaseTest
    {
        [TestMethod]
        public void SimpleVisualizerTest()
        {
            string templates =
                "method(type,name,locals,args,stats) ::= <<\n" +
                "public <type> <ick()> <name>(<args:{a| int <a>}; separator=\", \">) {\n" +
                "    <if(locals)>int locals[<locals>];<endif>\n" +
                "    <stats;separator=\"\\n\">\n" +
                "}\n" +
                ">>\n" +
                "assign(a,b) ::= \"<a> = <b>;\"\n" +
                "return(x) ::= <<return <x>;>>\n" +
                "paren(x) ::= \"(<x>)\"\n";

            writeFile(tmpdir, "t.stg", templates);
            STGroup group = new STGroupFile(Path.Combine(tmpdir, "t.stg"));
            STGroup.debug = true;
            DebugST st = (DebugST)group.getInstanceOf("method");
            st.impl.dump();
            st.add("type", "float");
            st.add("name", "foo");
            st.add("locals", 3);
            st.add("args", new String[] { "x", "y", "z" });
            ST s1 = group.getInstanceOf("assign");
            ST paren = group.getInstanceOf("paren");
            paren.add("x", "x");
            s1.add("a", paren);
            s1.add("b", "y");
            ST s2 = group.getInstanceOf("assign");
            s2.add("a", "y");
            s2.add("b", "z");
            ST s3 = group.getInstanceOf("return");
            s3.add("x", "3.14159");
            st.add("stats", s1);
            st.add("stats", s2);
            st.add("stats", s3);

            Inspect(st);
            //st.render();
        }

        private void Inspect(DebugST template)
        {
            Inspect(template, CultureInfo.CurrentCulture);
        }

        private void Inspect(DebugST template, CultureInfo culture)
        {
            Inspect(template, template.impl.nativeGroup.errMgr, culture, AutoIndentWriter.NoWrap);
        }

        private void Inspect(DebugST template, ErrorManager errorManager, CultureInfo culture, int lineWidth)
        {
            ErrorBuffer errors = new ErrorBuffer();
            template.impl.nativeGroup.setListener(errors);
            StringWriter @out = new StringWriter();
            ITemplateWriter wr = new AutoIndentWriter(@out);
            wr.setLineWidth(lineWidth);
            Interpreter interp = new Interpreter(template.groupThatCreatedThisInstance, culture);
            interp.Execute(wr, template); // render and track events
            TemplateVisualizer visualizer = new TemplateVisualizer(errorManager, template, @out.ToString(), interp, interp.getExecutionTrace(), errors.Errors);
            visualizer.Show();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace D2L.CodeStyle.Analysis {

    internal sealed class UtilsTests {

        private readonly Utils m_utils = new Utils();

        [Test]
        public void IsGeneratedCodefile_NotCSharpFile_False() {
            const string file = @"proj\random.js";

            Assert.IsFalse( m_utils.IsGeneratedCodefile( file ) );
        }

        [Test]
        public void IsGeneratedCodefile_CSharpFileNotInGeneratedFolder_False() {
            const string file = @"proj\random.cs";

            Assert.IsFalse( m_utils.IsGeneratedCodefile( file ) );
        }

        [Test]
        public void IsGeneratedCodefile_CSharpFileInGeneratedFolder_True() {
            const string file = @"proj\.generated\random.cs";

            Assert.IsTrue( m_utils.IsGeneratedCodefile( file ) );
        }

        [Test]
        public void IsGeneratedCodefile_CSharpFileInFolderWithGeneratedName_False() {
            const string file = @"proj.generated\random.cs";

            Assert.IsFalse( m_utils.IsGeneratedCodefile( file ) );
        }
    }
}
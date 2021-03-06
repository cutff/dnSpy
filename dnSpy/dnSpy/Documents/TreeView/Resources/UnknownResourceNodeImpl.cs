﻿/*
    Copyright (C) 2014-2016 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.IO;
using System.Threading;
using dnlib.DotNet;
using dnSpy.Contracts.Decompiler;
using dnSpy.Contracts.Documents.Tabs.DocViewer;
using dnSpy.Contracts.Documents.TreeView;
using dnSpy.Contracts.Documents.TreeView.Resources;
using dnSpy.Contracts.TreeView;
using dnSpy.Properties;

namespace dnSpy.Documents.TreeView.Resources {
	sealed class UnknownResourceNodeImpl : UnknownResourceNode, IDecompileSelf {
		public override Guid Guid => new Guid(DocumentTreeViewConstants.UNKNOWN_RESOURCE_NODE_GUID);

		public UnknownResourceNodeImpl(ITreeNodeGroup treeNodeGroup, Resource resource)
			: base(treeNodeGroup, resource) {
		}

		public override void WriteShort(IDecompilerOutput output, IDecompiler decompiler, bool showOffset) {
			base.WriteShort(output, decompiler, showOffset);
			var documentViewerOutput = output as IDocumentViewerOutput;
			if (documentViewerOutput != null) {
				documentViewerOutput.AddButton(dnSpy_Resources.SaveResourceButton, () => Save());
				documentViewerOutput.WriteLine();
				documentViewerOutput.WriteLine();
			}
		}

		public override string ToString(CancellationToken token, bool canDecompile) {
			var er = Resource as EmbeddedResource;
			if (er != null)
				return ResourceUtilities.TryGetString(new MemoryStream(er.GetResourceData()));
			return null;
		}

		public bool Decompile(IDecompileNodeContext context) {
			var er = Resource as EmbeddedResource;
			if (er != null)
				return ResourceUtilities.Decompile(context, new MemoryStream(er.GetResourceData()), er.Name);
			return false;
		}
	}
}

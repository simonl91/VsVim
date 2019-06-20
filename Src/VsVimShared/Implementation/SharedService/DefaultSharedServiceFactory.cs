﻿using System.Linq;
using Microsoft.FSharp.Core;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.Collections.Generic;
using Vim.Interpreter;

namespace Vim.VisualStudio.Implementation.SharedService
{
    internal sealed class DefaultSharedServiceFactory : ISharedServiceVersionFactory
    {
        private sealed class DefaultSharedService : ISharedService
        {
            WindowFrameState ISharedService.GetWindowFrameState()
            {
                return WindowFrameState.Default;
            }

            void ISharedService.GoToTab(int index)
            {
            }

            bool ISharedService.IsActiveWindowFrame(IVsWindowFrame vsWindowFrame)
            {
                return false;
            }

            void ISharedService.RunCSharpScript(IVimBuffer vimBuffer, CallInfo callInfo, bool createEachTime)
            {
                vimBuffer.VimBufferData.StatusUtil.OnError("csx not supported");
            }

            IEnumerable<SelectedSpan> ISharedService.GetSelectedSpans(ITextView textView)
            {
                var caretPoint = textView.Caret.Position.VirtualBufferPosition;
                var span = textView.Selection.StreamSelectionSpan;
                return new[] { new SelectedSpan(caretPoint, span) };
            }

            void ISharedService.SetSelectedSpans(ITextView textView, IEnumerable<SelectedSpan> selectedSpans)
            {
                var selectedSpan = selectedSpans.First();
                textView.Caret.MoveTo(selectedSpan.CaretPoint);
                if (selectedSpan.Length != 0)
                {
                    textView.Selection.Select(selectedSpan.StartPoint, selectedSpan.EndPoint);
                }
            }
        }

        VisualStudioVersion ISharedServiceVersionFactory.Version
        {
            get { return VisualStudioVersion.Unknown; }
        }

        ISharedService ISharedServiceVersionFactory.Create()
        {
            return new DefaultSharedService();
        }
    }
}

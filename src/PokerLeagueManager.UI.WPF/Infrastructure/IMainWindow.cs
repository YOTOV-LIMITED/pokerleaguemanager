using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PokerLeagueManager.UI.Wpf.Infrastructure
{
    public interface IMainWindow
    {
        void ShowView(object view);

        void ShowWarning(string title, string message);

        void ShowError(string title, string message);

        void SetWidth(int width);

        void SetHeight(int height);

        void SetMinWidth(int minWidth);

        void SetMinHeight(int minHeight);

        void SetMaxWidth(int maxWidth);

        void SetMaxHeight(int maxHeight);

        void SetWindowTitle(string title);
    }
}

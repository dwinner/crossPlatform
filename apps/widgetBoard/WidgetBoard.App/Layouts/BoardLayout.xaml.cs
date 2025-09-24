using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WidgetBoard.App.Layouts;

public partial class BoardLayout : Grid
{
   public BoardLayout()
   {
      InitializeComponent();
   }

   private void OnWidgetsChildAdded(object? sender, ElementEventArgs e)
   {
      throw new NotImplementedException();
   }
}
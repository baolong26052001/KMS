   M S B A M L   `   `   `  ����D  @Insurance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Insurance.ViewInsurance.View  5      &     Insurance.View.VirtualNumpadView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:Insurance.View     5      ��5	      �� 5
   
     �� ImageSource$  /Images/bg.jpg��6   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��2* �6   �� 5      $��* �6   .+�5   
   � $��1��6   .+�5       �� ��6   �� *DisplayText1+ �� Mode$ TwoWay=�5"      � 5#   
   $��2��6   .+�5$      �� ��6   �� *DisplayText2+$ TwoWay=�5&      � 5'   
   $��3��6   .+�5(      �� ��6   �� *DisplayText3+$ TwoWay=�5*      � 5+   
   $��4��6   .+�5,      �� ��6   �� *DisplayText4+$ TwoWay=�5.      �� 51   
   $��1��6   $��1��6"    �� Orientation$ 
Horizontal=�62   $��Center=�6K   $��Center=�6f   .+�52      ��  
  Numpad#��[  6   $��1��65   $��1��6c   ��6A   �� *NumberCommand+6c   �� 53      #��[  6   $��2��65   $��2��6c   ��6A   �� *NumberCommand+6c   �� 54      #��[  6   $��3��65   $��3��6c   ��6A   �� *NumberCommand+6c   55      �� 56   
   $��2��6   $��1��6"   $ 
Horizontal=�62   $��Center=�6K   $��Center=�6f   .+�57      �� #��[  6   $��4��65   $��4��6c   ��6A   �� *NumberCommand+6c   �� 58      #��[  6   $��5��65   $��5��6c   ��6A   �� *NumberCommand+6c   �� 59      #��[  6   $��6��65   $��6��6c   ��6A   �� *NumberCommand+6c   5:      �� 5;   
   $��3��6   $��1��6"   $ 
Horizontal=�62   $��Center=�6K   $��Center=�6f   .+�5<      �� #��[  6   $��7��65   $��7��6c   ��6A   �� *NumberCommand+6c   �� 5=      #��[  6   $��8��65   $��8��6c   ��6A   �� *NumberCommand+6c   �� 5>      #��[  6   $��9��65   $��9��6c   ��6A   �� *NumberCommand+6c   5?      �� 5@   
   $��4��6   $��1��6"   $ 
Horizontal=�62   $��Center=�6K   $��Center=�6f   .+�5A      �� #��[  6   $��C��65   
���   ���6A   �� *ClearCommand+6b   �� 5B      #��[  6   $��0��65   $��0��6c   ��6A   �� *NumberCommand+6c   �� 5C        NumpadDelete#��[ 6   $	��Del��6;   
���   ���6I   �� *DeleteCommand+6k   5D      5F      5G      
   M S B A M L   `   `   `  ����D  @Insurance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Insurance.ViewInsurance.View  5      $    Insurance.View.CashDepositView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:Insurance.View     5      ��5
      �� 5   
     �� ImageSource$  /Images/bg.jpg��6   64   .��5      �  � RowDefinitions 5   
   �� 5      6   �� 5      $	��650 �6   6+   �� 5      6   .+�5   
   � $��1��6    � ColumnDefinitions 5      �� 5      $��* �6#   6/   �� 5      $��* �6#   6/   .+�5      � $	��Top=�6   $��0 20 0 0q�6+   $��0��6=   $	��540��6M   $	��463��6Z   .+�5       �� $��45 0 0 0q�6   ���#FFFFFF$	��Exo)�6C   $��32(�6T   $
��Bold"�6b   $	��Top=�6t   $
��Left=�6�   Mệnh giá6�   �� 5"      $��0 0 75 0q�6   ���#FFFFFF$	��Exo)�6C   $��32(�6T   $
��Bold"�6b   $	��Top=�6t   $��Right=�6�   Số lượng6�   �� 5$      
������  �� CornerRadius$ 10 10 10 10��62   ���#0EAB00$	��447��6d   $��2q�6q   ._�5%      �  5&      �� 5'      $��* �6,   69   �� 5(      $��* �6,   69   �� 5)      $��* �6,   69   �� 5*      $��* �6,   69   .+�5,      �� $ 7.2 7.2 0 0��6!   $��0��6<   ���#0EAB00$��2q�6_   
������ ._�5-      ��  �� Orientation$ 
Horizontal=�6*   $��Center=�6C   .+�5.   "   �� $!��/Images/CashDeposit/50k.png��6(   $	��142��6M   $��64��6Y   $��37 0 0 0q�6e   �� 5/   "   $��Right=�6-   $��Stretch=�6I   ���#0EAB00$��3��6t   $�� 50 0 100 0q�6~   �� 50   "    �� 
IsReadOnly$
 True��6*   $��25(�6X   $
��Bold"�6f   $	��Exo)�6x   $��Center=�6�   ���#FFFFFF
������ $��2q�6�   
������ ��6<   �� *	BankNote1+6�   51      52      �� 54      $��1��6!   ���#0EAB00$��2q�6D   
������ ._�55      �� $ 
Horizontal=�6)   $��Center=�6B   .+�56   "   �� $"��/Images/CashDeposit/100k.png��6(   $	��142��6N   $��64��6Z   $��37 0 0 0q�6f   �� 57   "   $��Right=�6-   $��Stretch=�6I   ���#0EAB00$��3��6t   $�� 50 0 100 0q�6~   �� 58   "   $
 True��6*   $��25(�6X   $
��Bold"�6f   $	��Exo)�6x   $��Center=�6�   ���#FFFFFF
������ $��2q�6�   
������ ��6<   �� *	BankNote2+6�   59      5:      �� 5<      $��2��6!   ���#0EAB00$��2q�6D   
������ ._�5=      �� $ 
Horizontal=�6)   $��Center=�6B   .+�5>   "   �� $"��/Images/CashDeposit/200k.png��6(   $	��142��6N   $��64��6Z   $��37 0 0 0q�6f   �� 5?   "   $��Right=�6-   $��Stretch=�6I   ���#0EAB00$��3��6t   $�� 50 0 100 0q�6~   �� 5@   "   $
 True��6*   $��25(�6Y   $
��Bold"�6g   $	��Exo)�6y   $��Center=�6�   ���#FFFFFF
������ $��2q�6�   
������ ��6<   �� *	BankNote3+6�   5A      5B      �� 5C      $ 0 0 7.2 7.2��6!   $��3��6<   ���#0EAB00$��2q�6_   
������ ._�5D      �� $ 
Horizontal=�6)   $��Center=�6B   .+�5E   "   �� $"��/Images/CashDeposit/500k.png��6(   $	��142��6N   $��64��6Z   $��37 0 0 0q�6f   �� 5F   "   $��Right=�6-   $��Stretch=�6I   ���#0EAB00$��3��6t   $�� 50 0 100 0q�6~   �� 5G   "   $
 True��6*   $��25(�6X   $
��Bold"�6f   $	��Exo)�6x   $��Center=�6�   ���#FFFFFF
������ $��2q�6�   
������ ��6<   �� *	BankNote4+6�   5H      5I      5J      5L      � 5M      $��Bottom=�6    5N      �� 5O      $��* �6+   67   �� 5P      $��* �6+   67   .+�5R      �� $��0��6    $��Center=�60   $��Center=�6M   ���#FFFFFF$	��Exo)�6}   $��32(�6�   $
��Bold"�6�   Tổng tiền:5S   &   �� 5T      $��1��6   $��Center=�6.   $��Center=�6K   $
 True��6f   $��32(�5U   E   $
��Bold"�6S   $	��Exo)�6e   ���#FFFFFF
������ 
������ ��6   �� *_TotalDepositedMoney+6�   5[      5]      �� 5^      $��15 0 0 0q�6   $��Bottom=�6G   $
��Left=�6b   $��
Quay lại��6}      	bigButton#��[  6�   ��6'   �� *BackCommand+6�   � 5a      $��1��6   .+�5b      � .+�5c      ��  �� TextAlignment$ Center=�6=   ���#FFFFFF$	��Exo)�6j   $��32(�6{   $
��Bold"�6�   $��WrapWithOverflow=�6�   ��6    �� *PackageInfo+6�   5d      � 5f      $��0 60 0 0q�6   $	��Top=�6)   $	��301��6A   $	��441��6N    5h      �� 5i      $��2* �6+   68   �� 5j      $��3* �6+   68   .+�5m      �� $��Center=�6!   $��0��6<   .+�5n      �� $ Center=�6$   ���#FFFFFF$	��Exo)�6Q   $��32(�6b   $
��Bold"�6p   $��WrapWithOverflow=�6�   Cho tiền vào khe5o      �� 5p      $ Center=�6$   $��0 27 0 0q�6;   ���#FFFFFF$	��Exo)�6b   $��27(�6s   $
��Bold"�6�   $��WrapWithOverflow=�6�   để thanh toán5q      5r      � 5t      $��1��6   .+�5u      �� $	��Top=�6    $(��"/Images/CashDeposit/MoneyInput.png��69   $��58��6f   �� 5v      $	��Top=�6    $*��$/Images/CashDeposit/500kVertical.png��69   $	��104��6g   $	��235��6s   $��0 30 0 0q�6�   5x      5z      �� 5|      $��	0 0 0 150q�6   $��Bottom=�6/   $ Center=�6J   ���#FFFFFF$	��Exo)�6w   $��24(�6�   $
��Bold"�6�   $	��305��6�   $��80��6�   $��WrapWithOverflow=�6�   &$Chỉ nhận mệnh giá từ 50.0005}      �� 5      $��Bottom=�6h   $��Right=�6�   $��Tiếp tục��6�   $��10q�6�   
���2�2�
�������#��[  6�   ��6   �� *ConfirmCommand+y�6<   �� *VisibilityBtnConfirm+6�   5�      5�      5�      5�      
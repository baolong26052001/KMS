   M S B A M L   `   `   `  ����D  @Insurance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Insurance.ViewInsurance.View  5      (    "Insurance.View.DetailedPackageView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:Insurance.View     5      ��5	      �� 5
   
     �� ImageSource$  /Images/bg.jpg��6   .��5      �  � RowDefinitions 5   
   �� 5      �� 5      $	��650 �6   �� 5      .+�5   
   � $��1��6   .+�5      �� $��15 0 0 0q�6   $��25.28px(�6+   ���#FFFFFF$	��Exo)�6S   $
��Bold"�6e   Thông tin bảo hiểm6�   �� 5      $��Center=�6   $��	15 0 15 0q�64   .+�5      �� ���#0EAB00$	��448��6.   $	��990��6;    �� CornerRadius$ 10��6G    �� Padding$ 5q�6Z   ._�5      �� $��Center=�6!   .+�5      ��  �� Orientation$ 
Horizontal=�6%   $��	0 20 0 20q�6?   .+�5      �� 
�������$��30(�6X   $	��Exo)�6f   $
��Bold"�6w   ��6(   �� *PackageInfo+6w   5       �� 5"      
�������$ 5��64   $
��Left=�6E   $	��928��6`   $	��337��6l   ._�5#      �� $��1��6+   $��10q�68   $��Hidden=�6F    �� PanningMode$
 Both=�6k   $	��888��6~   .��5$   "   �� .+�5%   &   �� $#��Thông tin gói bảo hiểm:��60   
���   �$��28(�6b   $
��Bold"�6p   �� 5&   &   
���   �$��18(�6C    �� Padding$ 2q�6Q   ��5'   0   �� *PackageProvider+�� 5(   &   
���   �$��18(�6C   $ 2q�6Q   ��5)   0   �� *CreatedDate+�� 5*   &   
���   �$��18(�6C   $ 2q�6Q   ��5+   0   �� *ExpiredDate+�� 5.   &   $��Thông tin người mua��60   
���   �$��28(�6^   $
��Bold"�6l   $ 5q�6~   �� 5/   &   
���   �$��18(�6]   $ 2q�6k   ��60   �� *
FullName+6k   �� 50   &   
���   �$��18(�6`   $ 2q�6n   ��60   �� *DateOfBirth+6n   �� 51   &   
���   �$��18(�6[   $ 2q�6i   ��60   �� *Gender+6i   �� 52   &   
���   �$��18(�6Z   $ 2q�6h   ��60   �� *IDNum+6h   �� 55   &   $��Thông tin máy kiosk��60   
���   �$��28(�6^   $
��Bold"�6l   $ 5q�6~   �� 56   &   
���   �$��18(�6^   $ 2q�6l   ��60   �� *	KioskName+6l   �� 57   &   
���   �$��18(�6`   $ 2q�6n   ��60   �� *StationName+6n   �� 58   &   
���   �$��18(�6`   $ 2q�6n   ��60   �� *CompanyName+6n   �� 59   &   
���   �$��18(�6\   $ 2q�6j   ��60   �� *	Address+6j   �� 5:   &   
���   �$��18(�6Y   $ 2q�6g   ��60   �� *City+6g   �� 5;   &   
���   �$��18(�6]   $ 2q�6k   ��60   �� *
Location+6k   5<   #   5=      5>      5?      5@      5B      �� 5E      $��Bottom=�65   $��15 0 0 0q�6P   $
��Left=�6b   $��
Quay lại��6}      	bigButton#��[  6�   ��6   �� *BackCommand+6�   �� 5F      $��Bottom=�67   $��0 0 15 0q�6R   $��Right=�6d   $��Tiếp tục��6�   #��[  6�   ��6   �� *NextCommand+6�   5G      5J      5K      
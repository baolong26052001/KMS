   M S B A M L   `   `   `  ����D  @Insurance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Insurance	Insurance  5   	   9!clr-namespace:Insurance.ViewModelInsurance.ViewModel  5	          Insurance.View.HomeView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Insurance     5   	   0vm!clr-namespace:Insurance.ViewModel     5	      $	��768��5   
   $
��1024��6     � DataContext  5          Insurance.ViewModel.HomeVM  5   
   ��5      �� 5   
    �� ImageSource$ /Images/bg.jpg��6   64   $	��Exo)�5      .��5      �  � RowDefinitions 5   
   �� 5      �� 5      $	��650 �6   �� 5      .+�5   
   � $��1��6   .+�5      � $��	15 0 15 0q�6   .+�5       �� ���#FFFFFF$
��Bold"�61   $��20(�6C   Good morning,6`   �� 5!      ���#FFFFFF$
��Bold"�61   $��36(�6C    �� Padding$ 0 30 0 0q�6Q   Mr.A6j   �� 5"       �� Orientation$ Vertical=�6   $��Center=�64   $��Center=�6Q   .+�5#      �� $ 
Horizontal=�6!   .+�5$      �� $	��286��6!   $	��153��6-   $	��3.5q�6:   ���#0EAB00
������  � 	Resources 5%       �� 
TargetType �� Property �� Value��%j   '��       �� 5&   "   " ��6(   .�5'   &   ��  �@��CornerRadius$ 25 0 0 0��6E   6X   5(   #   5)      .��5*      �� $ 
Horizontal=�6)   .+�5+   "   �� $��/Images/home/Loan_Img.png��6(   $��75��6K   6X   �� 5,   "   $��Center=�6,   ���#FFFFFF$��36(�6\   $
��bold"�6j   $��16 8 0 0q�6|   Loan6�   5-      5.      �� 5/      $	��286��6!   $	��153��6-   $	��3.5q�6:   ���#0EAB00
������ .��50      �� $ 
Horizontal=�6)   .+�51   "   �� $��/Images/home/Pay_Img.png��6(   $��70��6J   6W   �� 52   "   $��Center=�6,   ���#FFFFFF$��36(�6\   $
��bold"�6j   $��16 8 0 0q�6|   Pay6�   53      54      �� 55      $	��286��6!   $	��153��6-   $	��3.5q�6:   ���#0EAB00
������  56      ��%j   '��       �� 57   "   " ��6(   .�58   &   ��  �@��CornerRadius$ 0 25 0 0��6E   6X   59   #   5:      .��5;      �� $ 
Horizontal=�6)   .+�5<   "   �� $"��/Images/home/History_Img.png��6(   $��65��6N   6[   �� 5=   "   $��Center=�6,   ���#FFFFFF$��36(�6\   $
��bold"�6j   $��16 8 0 0q�6|   	History6�   5>      5?      5@      �� 5A      $ 
Horizontal=�6!   .+�5B      �� $	��286��6!   $	��153��6-   $	��3.5q�6:   ���#0EAB00
������  5C      ��%j   '��       �� 5D   "   " ��6(   .�5E   &   ��  �@��CornerRadius$ 0 0 0 25��6E   6X   5F   #   5G      .��5H      �� $ 
Horizontal=�6)   .+�5I   "   �� $!��/Images/home/Saving_Img.png��6(   $��70��6M   6Z   �� 5J   "   $��Center=�6,   ���#FFFFFF$��36(�6\   $
��bold"�6j   $��16 8 0 0q�6|   Saving6�   5K      5L      �� 5M      $	��286��6!   $	��153��6-   $	��3.5q�6:   ���#0EAB00
������ .��5N      �� $ 
Horizontal=�6)   .+�5O   "   �� $#��/Images/home/WithDraw_Img.png��6(   $��70��6O   6\   �� 5P   "   $��Center=�6,   ���#FFFFFF$��36(�6\   $
��bold"�6j   $��5 8 0 0q�6|   
Withdraw6�   5Q      5R      �� 5S      $	��286��6!   $	��153��6-   $	��3.5q�6:   ���#0EAB00
������ ��5T       �� *InsuranceCommand+ 5V      ��%j   '��       �� 5W   "   " ��6(   .�5X   &   ��  �@��CornerRadius$ 0 0 25 0��6E   6X   5Y   #   5Z      .��5[      �� $ 
Horizontal=�6)   .+�5\   "   �� $$��/Images/home/Insurance_Img.png��6(   $��75��6P   6]   �� 5]   "   $��Center=�6,   ���#FFFFFF$��36(�6\   $
��bold"�6j   $��10 8 0 0q�6|   	Insurance6�   5^      5_      5`      5a      5b      5c      5k      5l      
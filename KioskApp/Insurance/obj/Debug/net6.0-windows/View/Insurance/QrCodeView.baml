   M S B A M L   `   `   `  ����D  @Insurance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Insurance.ViewInsurance.View  5          Insurance.View.QrCodeView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:Insurance.View     5      ��5	      �� 5
   
     �� ImageSource$  /Images/bg.jpg��6   .��5      �  � RowDefinitions 5   
   �� 5      �� 5      $	��650 �6   �� 5      .+�5   
   � $��1��6   $��15 0 0 0q�6    5      �� 5      �� 5      $	��650 �6    �� 5      .+�5      � $��1��6   $��Center=�6!   $��Center=�6<   $	��931��6Z   $	��520��6g   $��0 0 0 40q�6t   ��5      �� 5       $  /Images/Qr_bg.png��6!   .+�5#      �� $��Center=�6   $��Center=�68   .+�5$      �� $
��40px(�6    $	��565��60   $	��100��6<   ���#000000$
��Exo2)�6^   $
��Bold"�6q   $
��Wrap=�6�    �� TextAlignment$ Center=�6�   *(Vui lòng quét mã QR để thanh toán6�   �� 5%      $	��278��6   $	��278��6(   $��/Images/QrCode.png��65   $��0 20 0 0q�6Q   5&      5'      � 5)      $��1��6   .+�5*      �� $��Bottom=�69   $��Right=�6T   $��Tiếp tục��6p   $��0 0 0 0q�6�   
���2�2�
�������   	bigButton#��[  6�   ��6   �� *NextCommand+6�   �� 5+      $��Bottom=�69   $
��Left=�6T   $��
Quay lại��6o   #��[  6�   ��6   �� *BackCommand+6�   5,      5.      5/      50      
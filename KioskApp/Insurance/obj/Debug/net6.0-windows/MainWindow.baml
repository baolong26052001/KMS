   M S B A M L   `   `   `  ����D  @Insurance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Insurance	Insurance  5   	   9!clr-namespace:Insurance.ViewModelInsurance.ViewModel  5   	       Insurance.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  5   	   #localclr-namespace:Insurance  5   	   ,gif"http://wpfanimatedgif.codeplex.com  5	   	   *vm!clr-namespace:Insurance.ViewModel  5   	     � Title$  
MainWindow��5
   	   $	��768��6   $
��1024��6)    � WindowState$ 	Maximized=�5   
    � WindowStyle$
 None=�5   
   ��5      �� 5   
    �� ImageSource$ Images/bg.jpg��6   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $	��650 �6   �� 5      $��* �6   .+�5   
   � $��0��6   y�6   �� *VisibilityHeader+.+�5      �� $��
Bowlby One)�6   $��
ExtraBlack"�60   $
��Left=�6H   $��Center=�6c   $��40(�6~   ���#0EAB00$��15 0 0 0q�6�   ALM5      �� 5!      $��0 0 15 0q�6`      cancelButton#��[  6r   $��
Hủy bỏ��6�   $��Center=�6�   $��Right=�6�   y�6   �� *VisibilityBtnHead+��6>   �� *ShowPopupExit+6�   5$      � 5&   
   ��6   �� *RowContentIndex+��64   �� *RowContentSpan+.+�5'      �� ��6   �� *CurrentView+5(      � 5+   
   $��2��6   y�6   �� *VisibilityFooter+.+�5,      �� -   5         ��Name$ TimeTextBlock��5,      ���#FFFFFF$	��Exo)�6a   $��23(�6r   $��Center=�6�   $��15 0 0 0q�6�   ��6/   �� *CurrentTime+5-      �� 5.        bigButtonUserguide#��[ 6   $	��220��6A   $��40��6M   $ ��Hướng dẫn sử dụng��6Y   $��Right=�6�   $	��5 0q�6�   y�6u   �� *UserGuideVisible+��6�   �� *UserGuideCommand+6�   5/      �� 50   
   -   5         ��Name$ loadingOverlay_View��50      ���	#AA000000$��3��6z   y�6E   �� *LoadingOverlayVisibility_Main+._�51      �� $��Center=�6   $��Center=�66   .+�54      "  FontAwesome.WPF.ImageAwesome     Icon$ Refresh=�6"      Spin$
 True��61   $��60��6=   $��60��6I   	   
Foreground
	 � � �6T   56      57      � 5:   
   -   5        
 �Name$
 autoLogoutPopup��5:      $��Center=�6U    � 	StaysOpen$
 True��6h   $	��288��6y   $	��515��6�    � AllowsTransparency$
 True��6�   ��6)   �� *IsAutoLogoutPopupVisible+.��5;      �� ���#34393B���#34393B$��1q�6@    �� CornerRadius$ 5��6T   ._�5<      �� ���#34393B���#34393B��6   �� *ButtonPopupCommand+ � 	Resources 5=       �� 
TargetType �� Property �� Value��%c   '��       �� 5>      " ��6    .�5?      ��  �@��CornerRadius$ 4��6=   6I   5@      5A      .��5B      �� $��5q�6!   $��Center=�6,   
������ .+�5C      �� $&�� Tự động đăng xuất trong��6$   $��30(�6C   $
��Bold"�6Q   
�������$��Center=�6v   $��0 0 0 10q�6�   �� 5D      $��2q�6!   $	 100��65   $	��108��6H   $	��108��6U   ���#4E8D4D._�5E      �� $��30(�6(   $��Center=�66   $��Center=�6S   ���#0EAB00��5F   "   `� 5G   &    �� StringFormat$	 {0}��63   .1�5H   *   ��  �� Path$ TimeRemaining�62   5I   '   5K      5L      �� 5M      $
��Bold"�6$   
�������$��Center=�6I   $��30(�6f   $��0 10 0 0q�6t   Giây6�   5N      5O      5P      5Q      � 5T   
   -   5        $
 backtoWelcome��5T      $��Center=�6Q   $
 True��6d   $	��295��6u   $	��520��6�   $
 True��6�   ��6'   �� *IsBacktoWelcomeVisible+.��5U      �� ���#34393B���#34393B$��1q�6@   $ 5��6T   ._�5W      �� $��5q�6   $��Center=�6(   
������ .+�5X      �� $*��$Có lỗi trong quá trình xử lý��6    $��30(�6D   $
��Bold"�6R   
�������$��Center=�6w   $��0 0 0 10q�6�   �� 5Y      $
��Bold"�6    
�������$��Center=�6E   $��25(�6b   $��0 10 0 0q�6p   &$Xin lỗi vì sự bất tiện này6�   �� 5Z      $��Center=�6?   $��Trở về trang chủ��6\   $��0 20 0 0q�6w   
���2�2�
�������  	bigButton#��[ 6�   $	��280��6�   ��6   �� *CancelCommand+6�   5[      5]      5^      � 5_   
   -   5        $
 popupCancel��5_      $��Center=�6M   $
 True��6`   $	��300��6q   $	��560��6~   $
 True��6�   ��6%   �� *IspopupCancelVisible+.��5`      �� ���#34393B���#34393B$��1q�6@   $ 15��6T   ._�5b      �� $��5q�6   $��Center=�6(   
������ .+�5c      �� $��
THOÁT RA?��6    $��50(�61   $
��Bold"�6?   
�������$��Center=�6d   $��0 15 0 5q�6�   �� 5d      $6��0Bạn có chắc chắn muốn thoát ra không?��6    $��25(�6M   
�������$��Center=�6o   $��0 0 0 10q�6�   ]� 5e      $��Stretch=�6    $��
40 30 40 0q�6>   .+�5f      �� $
��Left=�6C   $	��Có��6^   
���2�2�
�������#��[ 6�   $	��160��6�   ��6!   �� *CancelCommand+6�   �� 5g      $��Right=�6C   $��Không��6_   
���2�2�
�������#��[ 6�   $	��160��6�   ��6!   �� *HidePopupExit+6�   5h      5i      5k      5l      5m      5o      
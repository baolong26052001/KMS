   M S B A M L   `   `   `  ����D  @Insurance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Insurance.ViewInsurance.View  5      )    #Insurance.View.BuyerInformationView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:Insurance.View     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      ��5
      �� 5   
     �� ImageSource$  /Images/bg.jpg��6   .��5      � .+�5   
   �  � RowDefinitions 5      �� 5      �� 5      $	��650 �6    �� 5       � ColumnDefinitions 5      �� 5      $��9* �6#   �� 5      $��* �6#   �� 5      $��9* �6#   .+�5      � $��1��6   $��0��6    $
��Left=�60   $	��Top=�6K   $��10 0 0 0q�6c   .+�5      �� $
��25px(�6   ���#FFFFFF$	��Exo)�6A   $
��Bold"�6R   Thông tin người mua6y   5       �� 5#      $��1��6   $��0��6&   $
��Left=�67   $��Center=�6R   $��20 0 0 0q�6n   $	��460��6�   .+�5%      �� $
��30px(�6   ���#FFFFFF$	��Exo)�6A   Họ và tên*6^   �� 5&      ${�False��6   $
��Left=�6,      textBox#��[  6G   $��0,0,0,10q�5'   0   ��6   �� *
FullName+60   � 5(      $��	0,10,0,10q�6    5)      �� 5*      $��2* �6+   �� 5+      $��* �6+   �� 5,      $��2* �6+   .+�50      �� $��0��6!   .+�51      �� $
��30px(�6$   ���#FFFFFF$	��Exo)�6I   Ngày sinh*53      (  "System.Windows.Controls.DatePicker  55        DatePickerStyle#���  6%     DatePickerCalendarStyle   CalendarStyle# �  56   !   
 � Tag 57   !   �� *DOB+{�58   !   �� *	IsEnabled+5:      �� 5=      $��2��6!   .+�5>      �� $
��30px(�6$   ���#FFFFFF$	��Exo)�6I   Giới tính*6f   �� 5I      #��[B��6#   ${�False��5K       5J      �� *Gender+.@�5M      �� $	��Nam��6+   �� 5N      $
��Nữ��6+   5O      5P      5R      �� 5U      $
��30px(�6   ���#FFFFFF$	��Exo)�6A   $��0,10,0,0q�6R   Địa chỉ*6n   �� 5V      ${�False��6   $
��Left=�6,   #��[  6G    �� TextWrapping$
 Wrap=�5W   %   $��0,0,0,10q�69   ��6   �� *	Address+69   �� 5j      $
��30px(�6   ���#FFFFFF$	��Exo)�6A   $��0,10,0,0q�6R   Nghề nghiệp*6r   �� 5k      #��[B��6   $ Nhấn vào để chọn��5l       �� SelectedValuePath$ Content��5n      $��0,0,0,40q�67    �� SelectedValue 5m      �� *
Occupation+.@�5p      �� $��Công nhân��6#   �� 5q      $��Nhân viên văn phòng��6#   �� 5r      $��Giáo viên��6#   �� 5s      $��Khác��6#   5t      5u      � 5x      $��1��6   $��1��6    .+�5y      �� 	 �� X1$ 30��6   		 �� X2$	 30��6!   	
 �� Y1$
 14��6)   	 �� Y2$	 600��61   ���#8B8989$��4��6L   5z      �� 5}      $��1��6   $��2��6&   $
��Left=�67   $��Center=�6R   $��10 0 0 0q�6o   $	��460��6�   .+�5      �� $
��30px(�6   ���#FFFFFF$	��Exo)�6A   
Số CCCD*6\   �� 5�      ${�False��6   $
��Left=�6,   #��[  6G   $��0,0,0,10q�5�   -   ��6   �� *IDNum+6-   �� 5�      $
��30px(�6   ���#FFFFFF$	��Exo)�6A    �� Padding$ 0,8,0,0q�6R   Mã số thuế(nếu có)*6y   �� 5�      -   5         ��Name$ tbTax��5�      #��[  6)    � ToolTip$ 5��5�   
   $��0,0,0,9q�6   ��5�   
   �� *Tax+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�.��5�   '   *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5�      +  %Microsoft.Xaml.Behaviors.EventTrigger  5�         	EventName$ 
MouseEnter��6)   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5�      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 64   �� *TextboxCommand+   CommandParameter 6W   ��  �� ElementName$ tbTax��5�      .��5�   .   5�      �� 5�      $
��30px(�6   ���#FFFFFF$	��Exo)�6A   $ 0,10,0,0q�6R   Email*6m   �� 5�      ${�False��6   $
��Left=�6,   #��[  6G   $��0,0,0,10q�5�   $   ��6   �� *Email+6$   �� 5�      $
��30px(�6   ���#FFFFFF$	��Exo)�6A   $��0,10,0,0q�6R   Số điện thoại*6t   �� 5�      ${�False��6   $
��Left=�6,   #��[  6G   $��0,0,0,40q�5�   '   ��6   �� *
PhoneNum+6'   5�      � 5�      $��1��6   $��2��6    $��0 0 15 0q�60   .+�5�      �� $��Bottom=�69   $��Right=�6T   $��Tiếp tục��6p   $��0 0 0 0q�6�   
���2�2�
�������  	bigButton#��[ 6�   ��6   �� *NextCommand+6�   5�      � 5�      $��1��6   $��0��6    $��15 0 0 0q�60   .+�5�      �� $��Bottom=�69   $
��Left=�6T   $��
Quay lại��6o   $��0 80 0 0q�6�   
���2�2�
�������#��[ 6�   ��6   �� *BackCommand+6�   5�      5�      5�      5�      
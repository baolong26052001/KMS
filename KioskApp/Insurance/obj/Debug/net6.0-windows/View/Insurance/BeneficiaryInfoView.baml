   M S B A M L   `   `   `  ����D  @Insurance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Insurance.ViewInsurance.View  5      (    "Insurance.View.BeneficiaryInfoView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:Insurance.View     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      ��5
      �� 5   
     �� ImageSource$  /Images/bg.jpg��6   .��5      � .+�5   
   � y�6   �� *VisibilityTextbox+ � RowDefinitions 5      �� 5      �� 5      $	��650 �6    �� 5       � ColumnDefinitions 5      �� 5      $��9* �6#   �� 5      $��* �6#   �� 5      $��9* �6#   .+�5      � $��1��6   $��0��6    $
��Left=�60   $	��Top=�6K   $��10 0 0 0q�6c   .+�5      �� $
��25px(�6   ���#FFFFFF$	��Exo)�6A   $
��Bold"�6R   $"Thông tin người thụ hưởng6   5      �� 5"      $��1��6   $��0��6&   $
��Left=�67   $��Center=�6R   $��20 0 0 0q�6n   $	��460��6�   .+�5%      �� $
��30px(�6   ���#FFFFFF$	��Exo)�6A   Họ và tên*6^   �� 5&      -   5         ��Name$ tbName��5&         textBox#��[  6*    � ToolTip$ 1��5(      $��0,0,0,10q�6&   ��5'      �� *TBName+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�.��5(   8   *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5)      +  %Microsoft.Xaml.Behaviors.EventTrigger  5*         	EventName$ 
MouseEnter��6)   *  $Microsoft.Xaml.Behaviors.TriggerBase	   Actions.	 5+      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction  
   Command
 64   �� *TextboxCommand+   CommandParameter 6W   ��  �� ElementName$ tbName��5,      .��5-   .   5.      � 50      $��	0,10,0,10q�6    51      �� 52      $��2* �6+   �� 53      $��* �6+   �� 54      $��2* �6+   .+�58      �� $��0��6!   .+�59      �� $
��30px(�6$   ���#FFFFFF$	��Exo)�6I   Ngày sinh*5;      (  "System.Windows.Controls.DatePicker  5=        DatePickerStyle#���  6%     DatePickerCalendarStyle   CalendarStyle# �  5>      
 � Tag 5?      �� *DOB+   SelectedDate 5@      �� *DOB+5B      �� 5E      $��2��6!   .+�5F      �� $
��30px(�6$   ���#FFFFFF$	��Exo)�6I   Giới tính*6f   �� 5O      #��[B��6#   $ Giới tính��5P       �� SelectedValuePath$ Content��5R       �� SelectedValue 5Q      �� *Gender+.@�5T      �� $	��Nam��6+   �� 5U      $
��Nữ��6+   5V      5W      5Y      �� 5\      $
��30px(�6   ���#FFFFFF$	��Exo)�6A   $��0,10,0,0q�6R   Nghề nghiệp*6r   �� 5f      #��[B��6   $ Nhấn vào để chọn��5g      $ Content��5i      $��0,0,0,10q�6(    5h      �� *TBJob+.@�5k      �� $��Công nhân��6#   �� 5l      $��Nhân viên văn phòng��6#   �� 5m      $��Giáo viên��6#   �� 5n      $��Khác��6#   5o      �� 5r      $
��30px(�6   ���#FFFFFF$	��Exo)�6A   $��0,10,0,0q�6R   Quan hệ với người mua*6|   �� 5|      #��[B��6   $ Nhấn vào để chọn��5}      $ Content��5      $��0,0,0,40q�63    5~      �� *Relationship+.@�5�      �� $��Cha/mẹ��6#   �� 5�      $��Ông/bà��6#   �� 5�      $��Anh/Chị/Em��6#   �� 5�      $��Khác��6#   5�      5�      � 5�      $��1��6   $��1��6    .+�5�      �� 	 �� X1$ 30��6   	 �� X2$ 30��6!   	 �� Y1$ 14��6)   	 �� Y2$	 600��61   ���#8B8989$��4��6L   5�      �� 5�      $��1��6   $��2��6&   $
��Left=�67   $��Center=�6R   $��10 0 0 0q�6o   $	��460��6�   .+�5�      �� $
��30px(�6   ���#FFFFFF$	��Exo)�6A   Địa chỉ*6\   �� 5�      -   5        $ tbAdress��5�      #��[  6,   $ 2��5�      $��0,0,0,10q�6   ��5�      �� *
TBAdress+$ TwoWay=�$ PropertyChanged=�.��5�   #    5�        5�      $ 
MouseEnter��6)   .	 5�        
 64   �� *TextboxCommand+ 6W   �� $ tbAdress��5�      .��5�   .   5�      �� 5�      $
��30px(�6   ���#FFFFFF$	��Exo)�6A   $��0,10,0,0q�6R   
Số CCCD*6n   �� 5�      -   5        $
 tbID��5�      #��[  6(   $ 4��5�      $��0,0,0,10q�6   ��5�      �� *TBID+$ TwoWay=�$ PropertyChanged=�.��5�   0    5�        5�      $ 
MouseEnter��6)   .	 5�        
 64   �� *TextboxCommand+ 6W   �� $
 tbID��5�      .��5�   .   5�      �� 5�      $
��30px(�6   ���#FFFFFF$	��Exo)�6A    �� Padding$ 0,10,0,0q�6R   Email*6m   �� 5�      -   5        $ tbEmail��5�      #��[  6+   $ 6��5�      $��0,0,0,10q�6   ��5�      �� *	TBEmail+$ TwoWay=�$ PropertyChanged=�.��5�   0    5�        5�      $ 
MouseEnter��6)   .	 5�        
 64   �� *TextboxCommand+ 6W   �� $ tbEmail��5�      .��5�   .   5�      �� 5�      $
��30px(�6   ���#FFFFFF$	��Exo)�6A   $��0,10,0,0q�6R   Số điện thoại*6t   �� 5�      -   5        $ tbPhone��5�      #��[  6+   $ 7��5�      $��0,0,0,40q�6   ��5�      �� *	TBPhone+$ TwoWay=�$ PropertyChanged=�.��5�   0    5�        5�      $ 
MouseEnter��6)   .	 5�        
 64   �� *TextboxCommand+ 6W   �� $ tbPhone��5�      .��5�   .   5�      5�      � 5�      $��1��6   $��2��6    $��0 0 15 0q�60   .+�5�      �� $��Bottom=�69   $��Right=�6T   $��Tiếp tục��6p   $��0 0 0 0q�6�   
���2�2�
�������  	bigButton#��[ 6�   ��6   �� *NextCommand+6�   5�      � 5�      $��1��6   $��0��6    $��15 0 0 0q�60   .+�5�      �� $��Bottom=�69   $
��Left=�6T   $��
Quay lại��6o   $��0 80 0 0q�6�   
���2�2�
�������#��[ 6�   ��6   �� *BackCommand+6�   5�      5�      5�      5�      
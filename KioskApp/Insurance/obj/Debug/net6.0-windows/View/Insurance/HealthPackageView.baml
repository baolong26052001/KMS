   M S B A M L   `   `   `  ����D  @Insurance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Insurance.ViewInsurance.View  5      &     Insurance.View.HealthPackageView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:Insurance.View     5      ��5	      �� 5
   
     �� ImageSource$  /Images/bg.jpg��6   .��5      �  � RowDefinitions 5   
   �� 5      �� 5      $	��650 �6   �� 5       � ColumnDefinitions 5   
   �� 5      $	��425 �6   �� 5      .+�5   
   � $��1��6   $��	20 0 15 0q�6   .+�5      �� $��25.28px(�6   ���#FFFFFF$	��Exo)�6@   $
��Bold"�6R   Health Insurance6w   �� 5      $��0 70 0 0q�6   $
��18px(�6*   ���#FFFFFF$	��Exo)�6O   $
��Bold"�6a   Package in use:6�   �� 5      -   5         ��Name$ 
StackPanel��5      $��Center=�6-   $��0��6I   $��2��6Y   5       5"      � 5'   
   $��Center=�6   $��1��6*   $��1��6:   .+�5(      �� .+�5)      �� ���#0EAB00 �� CornerRadius$ 10��6.   $��	24 0 15 0q�6@   ._�5*      �� .+�5+      �� $ ��Quyền lợi bảo hiểm��6$   
�������$��24(�6Q   $	��Exo)�6_   $
��Bold"�6p    �� Padding$ 20 5 0 0q�6�   �� 5-      
�������$ 10��64    �� Padding$ 10q�6F   $��
20 5 20 20q�6S   ._�5.      �� $
��Auto=�6+   $��Center=�6N   $
��True��6p    �� PanningMode$ HorizontalOnly=�6�    J� SnapsToDevicePixels$
 True��6�   	 � DataContext	 6�   �� .��5/   "   �� -   5        $ stackPanelBenefit��5/   -   $	��400��6H   $��10q�6U   $	��Top=�6a   .+�51   .   �� $��5q�69   .+�52   2   ]� .+�53   6   �� -   5        
 ��Name$
 Benefit1��53   @   $)��#1. Death due to illness or disease:��6R   
���   �$	��Exo)�6�   $
��Bold"�6�   $��16(�6�   �� 54   6   -   5        $
 	Coverage1��54   @   $��10.000.000 VND��6S   
���   �$
��Bold"�6|   $	��Exo)�6�   $��16(�6�   $��Right=�6�   55   3   56   /   �� 57   .   ���#000000$��1��6G   $��5 0 5 0q�6R   �� 5>   .   $��5q�69   .+�5?   2   ]� .+�5@   6   �� -   5        $
 Benefit2��5@   @   $��0��6R   $��2. Hospital fee support: ��6b   
���   �$
��Bold"�6�   $��16(�6�   �� 5A   6   -   5        $
 	Coverage2��5A   @   $��1��6S   $�� 3.000.000 VND��6c   
���   �$
��Bold"�6�   $��16(�6�   $��Right=�6�   5B   3   �� 5C   2   -   5        $
 DescriptionBenefit2��5C   <   $��10 0 0 0q�6Y   $��0��6k   $
��Wrap=�6{   $	��300��6�   $���zMaximum 100 days/year, each treatment should not exceed 15 days.The maximum number of days in the ICU is 30 days per year.��6�   
���   �$ 5q�60  $
��Left=�6<  � 5E   2   -   5         �Name$ gridBenefitDetails��5E   I   $��10 0 0 0q�67    5F   6   �� 5G   :   $
��Auto �6K   �� 5H   :   $��* �6K   .+�5K   6   �� $��0��6@   $1��+ Support amount per day of hospitalization:��6P   
���   �$ 0q�6�   $
��Bold"�6�   �� 5L   6   $��1��6@   $��
20.000 VND��6P   
���   �$��Right=�6u   5M   3   � 5P   2   $��10 0 0 0q�67    5Q   6   �� 5R   :   $
��Auto �6K   �� 5S   :   $��* �6K   .+�5V   6   �� $��0��6@   $!��If hospitalized in the ICU:��6P   
���   �$ 5q�6�   �� 5W   6   $��1��6@   $��
40.000 VND��6P   
���   �$��Right=�6u   $��Center=�6�   5X   3   5Y   /   �� 5[   .   ���#000000$��1��6G   $��5 0 5 0q�6R   �� 5]   .   $��5q�69   .+�5^   2   ]� .+�5_   6   �� -	   5        $
 Benefit3��5_   @   $#��3. Support for surgery fees: ��6R   
���   �$
��Bold"�6�   $��16(�6�   �� 5`   6   -
   5        $
 	Coverage3��5`   @   $��1��6S   $�� 1.000.000 VND��6c   
���   �$
��Bold"�6�   $��16(�6�   $��Right=�6�   5a   3   � 5c   2   $��10 0 0 0q�67    5d   6   �� 5e   :   $
��Auto �6K   �� 5f   :   $��* �6K   .+�5h   6   �� $��0��6@   $.��(Amount per time, maximum 2 time per year��6P   
���   �$ 5q�6�   �� 5i   6   $��1��6@   $��500.000 VND��6P   
���   �$��Right=�6v   5j   3   5l   /   5n   #   5o      5p      5q      5r      5s      �� 5t      $
��left=�6   $��Stretch=�64   ���#BBB7B7$��1��6_   $��
 0 68 0 32q�6i   5u      �� 5y   
   $��1��6   $��1��6"   $��Right=�62    �� Orientation$ 
Horizontal=�6N   $��Bottom=�6g   $��0,0,15,0q�6�   .+�5z      �� $	��Mua��69   
���2�2�
�������   	bigButton#��[  6r   ��6   �� *PurchaseCommand+6r   5{      5}      5~      
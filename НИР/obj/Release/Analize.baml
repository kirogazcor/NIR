   M S B A M L   `   `   `  ���� A  =НИР, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:НИРНИР      НИР.Analize   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     &localclr-namespace:НИР       � Title$)  #Анализ результатов��$	��600��$	��800�� � Icon$ Resources/Icon2.png�� � WindowStartupLocation$ CenterScreen=�$	��400��$	��300��.��� .+���  � 	Resources  �� 
TargetType �� Property �� Value��%X   '��       '� "     �� " ��.���  � ��$ 2���� " �.���  � ��$ 2��.@��� $+��%Количество контуров��.���� $��10q�$��2q�
�������._��� -    ��Name$ 	CanvasNum��$��10q�.+�� -   $��NumLine��
����  � � Points �� *
Graphici+� -   $��NoiceNumLine��
���  �� �� *NoiceGraphici+� 	 � RowDefinitions	 �� $��* ��� $��* �
 � ColumnDefinitions
 �� $��* ��� $��* �.+��� $��0��$��0��Без шума�� $��1��$��0��С шумом�� $��0��$��1��
����  �	 �� X1$ 5��	 �� X2$ 40��	 �� Y1$ 10��	 �� Y2$ 10���� $��1��$��1��
���  ��$ 5��$ 40��$ 10��$ 10���� $ 0��$ 0��$ 40��
���   �$��1�� ��  �� ElementName$ 	CanvasNum�� �� Path$ ActualHeight��� $ 0��
���   �$��1�� �� $ 	CanvasNum��$ ActualWidth� �� $ 	CanvasNum��$ ActualHeight� �� $ 	CanvasNum��$ ActualHeight��� $2��,Сумма площадей контуров��.���� $��10q�$��2q�
�������._��� -   $ 
CanvasArea��$��10q�.+�� -   $��AreaLine��
����  � �� *
Graphici+� -   $��NoiceAreaLine��
���  �� �� *NoiceGraphici+� 	 �� $��* ��� $��* �
 �� $��* ��� $��* �.+��� $��0��$��0��Без шума�� $��1��$��0��С шумом�� $��0��$��1��
����  �$ 5��$ 40��$ 10��$ 10���� $��1��$��1��
���  ��$ 5��$ 40��$ 10��$ 10���� $ 0��$ 0��$ 40��
���   �$��1�� �� $ 	CanvasNum��$ ActualHeight��� $ 0��
���   �$��1�� �� $ 	CanvasNum��$ ActualWidth� �� $ 	CanvasNum��$ ActualHeight� �� $ 	CanvasNum��$ ActualHeight��� $6��0Сумма периметров контуров��.���� $��10q�$��2q�
�������._��� -   $ CanvasPerimeter��$��10q�.+�� -   $��	PerimLine��
����  � �� *
Graphici+� -	   $��NoicePerimLine��
���  �� �� *NoiceGraphici+� 	 �� $��* ��� $��* �
 �� $��* ��� $��* �.+��� $��0��$��0��Без шума�� $��1��$��0��С шумом�� $��0��$��1��
����  �$ 5��$ 40��$ 10��$ 10���� $��1��$��1��
���  ��$ 5��$ 40��$ 10��$ 10���� $ 0��$ 0��$ 40��
���   �$��1�� �� $ 	CanvasNum��$ ActualHeight��� $ 0��
���   �$��1�� �� $ 	CanvasNum��$ ActualWidth� �� $ 	CanvasNum��$ ActualHeight� �� $ 	CanvasNum��$ ActualHeight�
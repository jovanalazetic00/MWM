   M S B A M L   `   `   `  ����I  ENetworkService, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:NetworkService.ViewsNetworkService.Views  5      C&clr-namespace:NetworkService.ViewModelNetworkService.ViewModel  5      ;"clr-namespace:NetworkService.ModelNetworkService.Model  5      7 clr-namespace:NetworkService.VMLNetworkService.VML  5	      /    )NetworkService.Views.MeasurementGraphView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:NetworkService.Views     5      <	viewModel&clr-namespace:NetworkService.ViewModel     5      3data"clr-namespace:NetworkService.Model     5      0vml clr-namespace:NetworkService.VML     5	      )   #NetworkService.VML.ViewModelLocator    AutoHookedUpViewModel  . 
�������.��5      �  � RowDefinitions 5   
   �� 5      $	��350 �6   6+   �� 5      $��* �6   6)   .+�5   
   � $��0��6   .+�5      �� $��	10 45 0 0q�6   $	��5.0��6'   $��13(�65   $
��Bold"�6C   
�������6U   �� 5      $��	10 70 0 0q�6   $	��4.5��6'   $��13(�65   $
��Bold"�6C   
�������6U   �� 5      $��	10 95 0 0q�6   $	��4.0��6'   $��13(�65   $
��Bold"�6C   
�������6U   �� 5      $��
10 120 0 0q�6   $	��3.5��6(   $��13(�66   $
��Bold"�6D   
�������6V   �� 5      $��
10 145 0 0q�6   $	��3.0��6(   $��13(�66   $
��Bold"�6D   
�������6V   �� 5      $��
10 170 0 0q�6   $	��2.5��6(   $��13(�66   $
��Bold"�6D   
�������6V   �� 5      $��
10 195 0 0q�6   $	��2.0��6(   $��13(�66   $
��Bold"�6D   
�������6V   �� 5      $��
10 220 0 0q�6   $	��1.5��6(   $��13(�66   $
��Bold"�6D   
�������6V   �� 5      $��
10 245 0 0q�6   $	��1.0��6(   $��13(�66   $
��Bold"�6D   
�������6V   �� 5      $��
10 270 0 0q�6   $	��0.5��6(   $��13(�66   $
��Bold"�6D   
�������6V   �� 5      $��
10 295 0 0q�6   $	��0.0��6(   $��13(�66   $
��Bold"�6D   
�������6V   �� 5      $��	50,45,5,5q�6   
�������.+�5       �� $��Bottom=�6�    �� Left$ 75��6�    �� Bottom$ 35��6�   $��25��6�   ��6   �� *H1+ �� UpdateSourceTrigger$ PropertyChanged=���6W   �� *C1+$ PropertyChanged=�6�   �� 5!      $��Bottom=�6�   $	 230��6�   $ 35��6�   $��25��6�   ��6   �� *H2+$ PropertyChanged=���6W   �� *C2+$ PropertyChanged=�6�   �� 5"      $��Bottom=�6�   $	 380��6�   $ 35��6�   $��25��6�   ��6   �� *H3+$ PropertyChanged=���6W   �� *C3+$ PropertyChanged=�6�   �� 5#      $��Bottom=�6�   $	 530��6�   $ 35��6�   $��25��6�   ��6   �� *H4+$ PropertyChanged=���6W   �� *C4+$ PropertyChanged=�6�   �� 5$      $��Bottom=�6�   $	 670��6�   $ 35��6�   $��25��6�   ��6   �� *H5+$ PropertyChanged=���6W   �� *C5+$ PropertyChanged=�6�   5%      �� 5&      $��100 310 0 0q�6   $��8(�6e   $
��Bold"�6r   ��6)   �� *D1+$ PropertyChanged=�6r   �� 5'      $��250 310 0 0q�6   $��8(�6e   $
��Bold"�6r   ��6)   �� *D2+$ PropertyChanged=�6r   �� 5(      $��400 310 0 0q�6   $��8(�6e   $
��Bold"�6r   ��6)   �� *D3+$ PropertyChanged=�6r   �� 5)      $��550 310 0 0q�6   $��8(�6e   $
��Bold"�6r   ��6)   �� *D4+$ PropertyChanged=�6r   �� 5*      $��690 310 0 0q�6   $��8(�6e   $
��Bold"�6r   ��6)   �� *D5+$ PropertyChanged=�6r   5+      � 5,   
   $��1��6   �� 5-   
   $��97,10,516,313q�6Z   ��6   �� *List+ �� SelectedItem 60   �� *SelectedListItem+6Z   �� 5.   
   $��500,10,225,313q�6/    � ToolTip$- 'Iscrtajte grafikone za odabrani entitet��6G    ^� ShowDuration$
 5000��6y   $
��Show��6�   ��6   �� *	ShowGraph+6�   5/      50      
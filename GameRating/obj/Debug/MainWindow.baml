   M S B A M L   `   `   `  ����  
GameRating    GameRating.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          �Name$  MainWindow1��5   	    � Title$ Team Rating��5   	   $	��335��6   $	��453��6*   $	��250��6R    � Icon$ LoL_icon.ico��6b   .��5      � $��0,0,0,3q�6    � ColumnDefinitions 5   
   �� 5       � RowDefinitions 5	   
   �� 5
      .+�5   
   �� -   5         ��Name$ loadHistoryBt��5      $��Load history��6(   $
��Left=�6?   $��	10,38,0,0q�6Z   $	��Top=�6m   $��75��6�   $��22��6�   �� 5   
   -   5         ��Name$ SummonerInputBox��5      $
��Left=�6,   $��23��6G   $��	10,10,0,0q�6S    �� TextWrapping$
 Wrap=�6f   $��Enter summoner name��6z   $	��Top=�6�   $	��182��6�   $��0��6�   6�   �� 5   
   -   5         ��Name$ 	regionBox��5      $��Right=�6&   $	��Top=�6B   $	��107��6Z   $��	0,11,10,0q�6f   $��22��6y   �� 5   
   -   5        	 ��Name$	 keyLabel��5      $��Label��6"   $��Right=�62   $��	0,38,10,0q�6N   $	��Top=�6a   $��23��6y   $	��256��6�   $��Right=�6�   &   System.Windows.Controls.DataGrid  5   
   -   5        
  Name$
 historyView��5      $��10,66,10,10q�6(   �� 5   
   -   5        $ ldMoreBt��5      $��	Load more��6#   $
��Left=�67   $��	90,38,0,0q�6R   $	��Top=�6e   $��75��6}   6�   5      5      
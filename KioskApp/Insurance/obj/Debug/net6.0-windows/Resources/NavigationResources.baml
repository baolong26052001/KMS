   M S B A M L   `   `   `  ����D  @Insurance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Insurance.ViewModelInsurance.ViewModel  5      /clr-namespace:Insurance.ViewInsurance.View  5      E'clr-namespace:Insurance.VirtualKeyboardInsurance.VirtualKeyboard  5      E'clr-namespace:Insurance.VirtualKeyboardInsurance.VirtualKeyboard  5      %clr-namespace:Insurance	Insurance  5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� DataType&    Insurance.ViewModel.MainWindowVM   Insurance.MainWindow#   Insurance.ViewModel.WelcomeVM    Insurance.View.WelcomeView$   Insurance.ViewModel.VerifyIDVM!   Insurance.View.VerifyIDView(   "Insurance.ViewModel.VerifyFaceIDVM%   Insurance.View.VerifyFaceIDView(   "Insurance.ViewModel.VerifyIDBackVM!	   Insurance.View.VerifyIDBack+
   %Insurance.ViewModel.RegisterSuccessVM(   "Insurance.View.RegisterSuccessView    Insurance.ViewModel.HomeVM   Insurance.View.HomeView(   "Insurance.ViewModel.PersonalInfoVM%   Insurance.View.PersonalInfoView-  'Insurance.ViewModel.InsuranceProviderVM*   $Insurance.View.InsuranceProviderView#  Insurance.ViewModel.BaoVietVM$   Insurance.View.BaoVietHomeView(   "Insurance.ViewModel.AnBinhYenVuiVM%   Insurance.View.AnBinhYenVuiView!  Insurance.ViewModel.AnGiaVM   Insurance.View.AnGiaView%  Insurance.ViewModel.AnGiaPackVM"   Insurance.View.AnGiaPackView,  &Insurance.ViewModel.BuyerInformationVM)   #Insurance.View.BuyerInformationView(  "Insurance.ViewModel.ContractInfoVM%   Insurance.View.ContractInfoView+  %Insurance.ViewModel.DetailedPackageVM(   "Insurance.View.DetailedPackageView)   #Insurance.ViewModel.PaymentMethodVM&!    Insurance.View.PaymentMethodView'"   !Insurance.ViewModel.CashDepositVM$#   Insurance.View.CashDepositView"$  Insurance.ViewModel.QrCodeVM%   Insurance.View.QrCodeView-&  'Insurance.ViewModel.SelectBankAccountVM*'   $Insurance.View.SelectBankAccountView(  Insurance.ViewModel.OTPVM)   Insurance.View.OTPView**  $Insurance.ViewModel.BeneficiaryAddVM'+   !Insurance.View.BeneficiaryAddView+,  %Insurance.ViewModel.BeneficiaryInfoVM(-   "Insurance.View.BeneficiaryInfoView,.  &Insurance.ViewModel.InsuranceSuccessVM)/   #Insurance.View.InsuranceSuccessView)0  #Insurance.ViewModel.VirtualNumpadVM&1    Insurance.View.VirtualNumpadView12  +Insurance.VirtualKeyboard.VirtualKeyboardVM33   -Insurance.VirtualKeyboard.VirtualKeyboardView!4   Insurance.ViewModel.PopUpVM 5   Insurance.View.PopUpWindow&6    Insurance.ViewModel.SelectLoanVM#7   Insurance.View.SelectLoanView+8   %Insurance.ViewModel.EnterLoanAmountVM(9   "Insurance.View.EnterLoanAmountView$:   Insurance.ViewModel.LoanTermVM!;   Insurance.View.LoanTermView#<   Insurance.ViewModel.PayHomeVM =   Insurance.View.PayHomeView'>   !Insurance.ViewModel.LoanPaymentVM$?   Insurance.View.LoanPaymentView0@   *Insurance.ViewModel.InsuranceTransactionVM-A   'Insurance.View.InsuranceTransactionView,B   &Insurance.ViewModel.InsuranceHistoryVM)C   #Insurance.View.InsuranceHistoryView(D   "Insurance.ViewModel.ListWithdrawVM%E   Insurance.View.ListWithdrawView0F   *Insurance.ViewModel.FaceRecogStatusLoginVM-G   'Insurance.View.FaceRecogStatusLoginView0H   *Insurance.ViewModel.UGFaceRecog_SUStatusVM-I   'Insurance.View.UGFaceRecog_SUStatusView*J   $Insurance.ViewModel.UGFaceRecog_SUVM'K   !Insurance.View.UGFaceRecog_SUView'L   !Insurance.ViewModel.UGPesonInfoVM$M   Insurance.View.UGPesonInfoView'N   !Insurance.ViewModel.UGSuccessSUVM$O   Insurance.View.UGSuccessSUView&P    Insurance.ViewModel.UGVerifyIDVM#Q   Insurance.View.UGVerifyIDView%R   Insurance.ViewModel.UserGuideVM"S   Insurance.View.UserGuideView/T   )Insurance.ViewModel.UserGuildeFaceRecogVM,U   &Insurance.View.UserGuildeFaceRecogView�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      (vm!clr-namespace:Insurance.ViewModel  5      "vclr-namespace:Insurance.View  5      .vk'clr-namespace:Insurance.VirtualKeyboard  5      0vkvm'clr-namespace:Insurance.VirtualKeyboard  5      %	insuranceclr-namespace:Insurance  5      %�  (��       5	      *,  +)(�� 2     5      *, +)(�� d     5      *, +)(�� �     5      *, +)(�� �     5      *, +)(�� �     5!      *,
 +)(�� 1    5%      *, +)(�� c    5,      *, +)(�� �    5/      *, +)(�� �    53      *, +)(�� �    57      *, +)(�� 0    5;      *, +)(�� b    5?      *, +)(�� �    5C      *, +)(�� �    5G      *, +)(�� �    5K      *, +)(�� *    5O      *,  +)(�� \    5S      *," +)(�� �    5W      *,$ +)(�� �    5[      *,& +)(�� �    5_      *,( +)(�� $    5c      *,* +)(�� V    5g      *,, +)(�� �    5k      *,. +)(�� �    5o      *,0 +)(�� �    5{      *,2 +)(��     5      *,4 +)(�� P    5�      *,6 +)(�� �    5�      *,8 +)(�� �    5�      *,: +)(�� �    5�      *,< +)(��     5�      *,> +)(�� J    5�      *,@ +)(�� |    5�      *,B +)(�� �    5�      *,D +)(�� �    5�      *,F +)(��     5�      *,H +)(�� D    5�      *,J +)(�� v    5�      *,L +)(�� �    5�      *,N +)(�� �    5�      *,P +)(��     5�      *,R +)(�� >    5�      *,T +)�� 5	      "    6   .R�5
   
     5      �� 5      "   6   .R�5   
     5      �� 5      "   6   .R�5   
     5      �� 5      "   6   .R�5   
     5      �� 5      "   6   .R�5   
   	  6   5      �� 5!      "  
 6   .R�5"   
     5#      �� 5%      "   6   .R�5&   
     5'      �� 5,      "   6   .R�5-   
     6   5.      �� 5/      "   6   .R�50   
     51      �� 53      "   6   .R�54   
     55      �� 57      "   6   .R�58   
     59      �� 5;      "   6   .R�5<   
     5=      �� 5?      "   6   .R�5@   
     5A      �� 5C      "   6   .R�5D   
     5E      �� 5G      "   6   .R�5H   
     5I      �� 5K      "   6   .R�5L   
     5M      �� 5O      "    6   .R�5P   
   !  5Q      �� 5S      "  " 6   .R�5T   
   #  5U      �� 5W      "  $ 6   .R�5X   
   %  5Y      �� 5[      "  & 6   .R�5\   
   '  5]      �� 5_      "  ( 6   .R�5`   
   )  5a      �� 5c      "  * 6   .R�5d   
   +  5e      �� 5g      "  , 6   .R�5h   
   -  5i      �� 5k      "  . 6   .R�5l   
   /  5m      �� 5o      "  0 6   .R�5p   
   1  5q      �� 5{      "  2 6   .R�5|   
   3  5}      �� 5      "  4 6   .R�5�   
   5  5�      �� 5�      "  6 6   .R�5�   
   7  5�      �� 5�      "  8 6   .R�5�   
   9  5�      �� 5�      "  : 6   .R�5�   
   ;  5�      �� 5�      "  < 6   .R�5�   
   =  5�      �� 5�      "  > 6   .R�5�   
   ?  5�      �� 5�      "  @ 6   .R�5�   
   A  5�      �� 5�      "  B 6   .R�5�   
   C  5�      �� 5�      "  D 6   .R�5�   
   E  5�      �� 5�      "  F 6   .R�5�   
   G  5�      �� 5�      "  H 6   .R�5�   
   I  5�      �� 5�      "  J 6   .R�5�   
   K  5�      �� 5�      "  L 6   .R�5�   
   M  5�      �� 5�      "  N 6   .R�5�   
   O  5�      �� 5�      "  P 6   .R�5�   
   Q  5�      �� 5�      "  R 6   .R�5�   
   S  5�      �� 5�      "  T 6   .R�5�   
   U  5�      5�      
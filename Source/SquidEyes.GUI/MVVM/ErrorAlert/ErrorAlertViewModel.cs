//using System;
//using System.Diagnostics;
//using System.Text;
//using System.Windows;
//using System.Windows.Media.Imaging;
//using SquidEyes.Generic;
//using System.Diagnostics.Contracts;

//namespace SquidEyes.GUI
//{
//    public class ErrorAlertViewModel :
//        ViewModelBase<ErrorAlertViewModel, ErrorAlertModel, ErrorAlert>
//    {
//        public event EventHandler<NotifyArgs> OnErrorDetails;
//        public event EventHandler<NotifyArgs> OnSendErrorReport;
//        public event EventHandler<NotifyArgs> OnClose;

//        public ErrorAlertViewModel(ErrorAlertModel model)
//            : base(model)
//        {
//        }

//        //internal ErrorAlertViewModel(ErrorAlertModel model, BitmapImage appIcon,
//        //    ISupportInfo supportInfo, AppInfo appInfo, string title, 
//        //    bool checkPositionsAndOrders)
//        //{
//        //    Contract.Requires(model != null);
//        //    Contract.Requires(appIcon != null);
//        //    Contract.Requires(supportInfo != null);
//        //    Contract.Requires(title.IsTrimmed());

//        //    Model = model;

//        //    AppIcon = appIcon;
//        //    Model = model;
//        //    SupportInfo = supportInfo;
//        //    AppInfo = appInfo;
//        //    Title = title;

//        //    if (checkPositionsAndOrders)
//        //        PositionAndOrderVisibility = Visibility.Visible;
//        //    else
//        //        PositionAndOrderVisibility = Visibility.Collapsed;
//        //}

//        public BitmapImage AppIcon { get; private set; }
//        public string Title { get; private set; }
//        public ISupportInfo SupportInfo { get; private set; }
//        public AppInfo AppInfo { get; private set; }
//        public Visibility PositionAndOrderVisibility { get; private set; }

//        public DelegateCommand SupportEmailCommand
//        {
//            get
//            {
//                var sb = new StringBuilder();

//                sb.Append("mailto:");
//                sb.Append(SupportInfo.Email);
//                sb.Append("?subject=");
//                sb.Append(AppInfo.Product);
//                sb.Append(" Error (AlertCode: ");
//                sb.Append(Model.ErrorBlock.AlertCode);
//                sb.Append(")");

//                return new DelegateCommand(() => Process.Start(sb.ToString()));
//            }
//        }

//        public DelegateCommand SupportWebsiteCommand
//        {
//            get
//            {
//                return new DelegateCommand(
//                    () => Process.Start(SupportInfo.Website));
//            }
//        }

//        public DelegateCommand SendErrorReportCommand
//        {
//            get
//            {
//                return new DelegateCommand(() => Notify(OnSendErrorReport));
//            }
//        }

//        public DelegateCommand ErrorDetailsCommand
//        {
//            get
//            {
//                return new DelegateCommand(() => Notify(OnErrorDetails));
//            }
//        }

//        public DelegateCommand CloseCommand
//        {
//            get
//            {
//                return new DelegateCommand(() => Notify(OnClose));
//            }
//        }
//    }
//}

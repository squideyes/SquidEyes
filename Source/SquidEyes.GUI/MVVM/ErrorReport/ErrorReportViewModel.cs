//using System;
//using SquidEyes.Generic;
//using System.Diagnostics.Contracts;

//namespace SquidEyes.GUI
//{
//    public class ErrorReportViewModel :
//        ViewModelBase<ErrorReportViewModel, ErrorReportModel, ErrorReport>
//    {
//        public event EventHandler<NotifyArgs> OnSendErrorReport;
//        public event EventHandler<NotifyArgs> OnClose;
//        public event EventHandler<NotifyArgs> OnSelectAttachment;

//        ErrorReportViewModel(ErrorReportModel model)
//            : base(model)
//        {
//        }

//        //public ErrorReportViewModel(ErrorReportModel model, string title)
//        //{
//        //    Contract.Requires(model != null);
//        //    Contract.Requires(title.IsTrimmed());

//        //    Model = model;
//        //    Title = title;
//        //}

//        public string Title { get; private set; }

//        public DelegateCommand ClearAttachmentCommand
//        {
//            get
//            {
//                return new DelegateCommand(() => Model.AttachmentFileName = null);
//            }
//        }

//        public DelegateCommand SelectAttachmentCommand
//        {
//            get
//            {
//                return new DelegateCommand(() => Notify(OnSelectAttachment));
//            }
//        }

//        public DelegateCommand CloseCommand
//        {
//            get
//            {
//                return new DelegateCommand(() => Notify(OnClose));
//            }
//        }

//        public DelegateCommand SendErrorReportCommand
//        {
//            get
//            {
//                return new DelegateCommand(() => Notify(OnSendErrorReport));
//            }
//        }
//    }
//}

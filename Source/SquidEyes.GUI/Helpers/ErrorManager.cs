using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;
using SquidEyes.Generic;
using SquidEyes.Shared;
using Microsoft.Win32;
using Microsoft.WindowsAzure.StorageClient;
using PR = SquidEyes.GUI.Properties.Resources;
using System.Diagnostics.Contracts;

namespace SquidEyes.GUI
{
    public class ErrorManager
    {
        public ErrorManager(BitmapImage appIcon, ISupportInfo supportInfo,
            string title, string basePath, AppInfo appInfo, bool checkPositionsAndOrders)
        {
            Contract.Requires(appIcon != null);
            Contract.Requires(supportInfo != null);
            Contract.Requires(title.IsTrimmed());
            Contract.Requires(basePath.IsDirectory(true));
            Contract.Requires(appInfo != null);

            AppIcon = appIcon;
            SupportInfo = supportInfo;
            Title = title;
            BasePath = basePath;
            AppInfo = appInfo;
            CheckPositionsAndOrders = checkPositionsAndOrders;

            ErrorLogsPath = Path.Combine(basePath, "Logs", "Errors");
        }

        public string ErrorLogsPath { get; private set; }
        public BitmapImage AppIcon { get; private set; }
        public ISupportInfo SupportInfo { get; private set; }
        public string BasePath { get; private set; }
        public string Title { get; private set; }
        public AppInfo AppInfo { get; private set; }
        public bool CheckPositionsAndOrders { get; private set; }

        public void LogAndAlert(ErrorBlock errorBlock)
        {
            if (!Directory.Exists(ErrorLogsPath))
                Directory.CreateDirectory(ErrorLogsPath);

            var savedTo = Path.Combine(ErrorLogsPath, errorBlock.FileName);

            using (var writer = new StreamWriter(savedTo))
                writer.Write(errorBlock.ToDocument().ToXml());

            //var model = new ErrorAlertModel()
            //{
            //    ErrorBlock = errorBlock
            //};

            //var view = new ErrorAlertView();

            //var viewModel = new ErrorAlertViewModel(
            //    model, AppIcon, SupportInfo, AppInfo, Title, CheckPositionsAndOrders);

            //viewModel.OnClose += (s, e) => view.Close();

            //viewModel.OnErrorDetails += (s, e) => ShowErrorDetails(view, savedTo);

            //viewModel.OnSendErrorReport += (s, e) => SendErrorReport(view, errorBlock);

            //view.DataContext = viewModel;

            //view.ShowDialog();
        }

        private void ShowErrorDetails(ErrorAlertView owner, string savedTo)
        {
            if (Modal.YesNoDialog(
                owner, "Confirmation", PR.ErrorDetailsConfirmation))
            {
                try
                {
                    Process.Start(savedTo);
                }
                catch
                {
                    Modal.WarningDialog(
                        null, "Warning",PR.ErrorDetailsDisplayError);
                }
            }
        }

        private bool? SendErrorReport(ErrorAlertView owner, ErrorBlock errorBlock)
        {
            //var model = new ErrorReportModel();

            //var view = new ErrorReportView();

            //var viewModel = new ErrorReportViewModel(model, Title);

            //viewModel.OnClose += (s, e) => view.Close();

            //string lastPath =
            //    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //viewModel.OnSelectAttachment += (s, e) =>
            //{
            //    var dialog = new OpenFileDialog();

            //    dialog.Filter = "Any Files (*.*)|*.*";
            //    dialog.FilterIndex = 0;
            //    dialog.Title = "Select an Attachment";

            //    dialog.InitialDirectory = lastPath;

            //    if (dialog.ShowDialog(owner) == true)
            //    {
            //        try
            //        {
            //            var fi = new FileInfo(dialog.FileName);

            //            if (fi.Length > ErrorReport.MaxAttachmentSize)
            //            {
            //                Modal.WarningDialog(owner, "Warning", 
            //                    PR.ErrorReportAttachmentTooBig, 
            //                    ErrorReport.MaxAttachmentSize.ToMB(0));

            //                return;
            //            }

            //            lastPath = fi.FullName;

            //            model.AttachmentFileName = fi.FullName;
            //        }
            //        catch (Exception error)
            //        {
            //            Modal.WarningDialog(owner, "Warning", error.Message);
            //        }
            //    }
            //};

            //viewModel.OnSendErrorReport += (s, e) =>
            //{
            //    if (string.IsNullOrEmpty(model.Message) &&
            //        string.IsNullOrWhiteSpace(model.AttachmentFileName))
            //    {
            //        if (!Modal.YesNoDialog(
            //            view, "Confirmation", PR.NoMessageNoAttachment))
            //        {
            //            return;
            //        }
            //    }

            //    view.DialogResult = true;

            //    view.Close();

            //    Exception error;

            //    if (WaitAndDoDialog.WaitAndDo(owner, "Sending Error Report", () =>
            //    {
            //        var report = new ErrorReport()
            //        {
            //            Guid = Guid.NewGuid(),
            //            ErrorBlock = errorBlock,
            //            Message = model.Message,
            //            FileName = model.AttachmentFileName
            //        };

            //        EnqueueErrorReport(report);
            //    },
            //    out error))
            //    {
            //        Modal.InfoDialog(
            //            owner, "Error Report Sent", PR.ErrorReportWasSent);
            //    }
            //    else
            //    {
            //        Modal.WarningDialog(owner, Title, PR.ErrorReportEnqueueError,
            //            SupportInfo.Email, SupportInfo.Phone, SupportInfo.Website);
            //    };
            //};

            //view.SetOwner(owner);

            //view.DataContext = viewModel;

            //return view.ShowDialog();

            return null;
        }

        private void EnqueueErrorReport(ErrorReport report)
        {
            if (string.IsNullOrWhiteSpace(report.Message))
                report.Message = null;

            if (string.IsNullOrWhiteSpace(report.FileName))
                report.FileName = null;

            report.Validate();

            if (report.FileName != null)
                UploadAttachment(report);

            //var queue = QueueHelper.GetQueue(
            //    Statics.AzureInfo.ConnStrings[ConnStringKind.Storage],
            //    QueueKind.ErrorReports.ToString().ToLower());

            //var xml = report.ToDocument().ToXml();

            //queue.AddMessage(new CloudQueueMessage(xml));
        }

        private void UploadAttachment(ErrorReport report)
        {
            const string BADATTACHMENTSIZE =
                "The \"{0}\" attachment was unexpectedly larger than {1}!";

            //var container = BlobHelper.GetContainer(
            //    Statics.AzureInfo.ConnStrings[ConnStringKind.Storage],
            //    ContainerKind.Attachments.ToString().ToLower());

            var fi = new FileInfo(report.FileName);

            if (fi.Length >= ErrorReport.MaxAttachmentSize)
            {
                throw new ApplicationException(string.Format(
                    BADATTACHMENTSIZE, report.FileName,
                    ErrorReport.MaxAttachmentSize.ToMB(0)));
            }

            var attachment = new byte[fi.Length];

            using (var stream = fi.Open(FileMode.Open, FileAccess.Read))
                stream.Read(attachment, 0, (int)fi.Length);

            //var blob = container.GetBlockBlobReference(report.BlobName);

            //blob.Properties.ContentType = report.FileName.ToMediaType();

            //blob.Properties.ContentMD5 = Convert.ToBase64String(attachment.ToMD5());

            //blob.Metadata.Add("AlertCode", report.ErrorBlock.AlertCode);
            //blob.Metadata.Add("Company", AppInfo.Company);
            //blob.Metadata.Add("Product", AppInfo.Product);
            //blob.Metadata.Add("Version", AppInfo.Version.ToVersionString());
            //blob.Metadata.Add("FileName", report.NameOnly);

            //var options = new BlobRequestOptions();

            //options.Timeout = TimeSpan.FromSeconds(5);

            //options.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(1));

            //blob.UploadByteArray(attachment, options);
        }
    }
}

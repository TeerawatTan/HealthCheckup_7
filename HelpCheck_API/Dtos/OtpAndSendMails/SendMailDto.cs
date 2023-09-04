using System.Collections.Generic;

namespace HelpCheck_API.Dtos.OtpAndSendMails
{
    public class MailRequest
    {
        /// <summary>
        ///  หัวข้อ
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        ///  ชื่อผู้ส่ง
        /// </summary>
        internal string Sender { get; set; }
        /// <summary>
        ///  อีเมลผู้ส่ง
        /// </summary>
        internal string SenderEmail { get; set; }
        /// <summary>
        ///  อีเมลผู้รับ
        /// </summary>
        public string ReceiverEmail { get; set; }
        /// <summary>
        ///  เนื้อหา (html format)
        /// </summary>
        public string Details { get; set; }
        public List<FileDetails> Attachments { get; set; }
    }
}
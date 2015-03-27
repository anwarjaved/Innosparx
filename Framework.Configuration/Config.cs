namespace Framework.Configuration
{
    using Framework.Serialization.Xml;

    [XmlElement(Name = "Configuration")]
    public sealed class Config
    {
        public Config()
        {
            this.Mail = new MailSettings();
            this.Amazon = new AmazonSettings();
            this.Application = new ApplicationSetting();
            this.Social = new SocialApiSetting();
        }

        public AmazonSettings Amazon { get; private set; }

        public MailSettings Mail { get; private set; }

        public ApplicationSetting Application { get; private set; }

        public SocialApiSetting Social { get; private set; }
    }
}
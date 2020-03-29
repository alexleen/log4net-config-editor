// Copyright © 2020 Alex Leendertsen

using System.ComponentModel;
using System.Net.Mail;
using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using static log4net.Appender.SmtpAppender;

namespace Editor.Definitions.Appenders
{
    internal class SmtpAppender : BufferingAppenderSkeleton
    {
        private readonly Password mPassword;
        private readonly RequiredStringProperty mUsername;

        private int mPasswordIndex;
        private int mUsernameIndex;

        public SmtpAppender(IElementConfiguration configuration)
            : base(configuration, true)
        {
            mUsername = new RequiredStringProperty("Username:", "username");
            mPassword = new Password("Password:", "password");
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Smtp;

        public override string Name => "SMTP Appender";

        protected override void AddAppenderSpecificProperties()
        {
            base.AddAppenderSpecificProperties();

            EnumProperty<SmtpAuthentication> auth = new EnumProperty<SmtpAuthentication>("Authentication:", 60, "authentication");
            auth.PropertyChanged += AuthenticationOnPropertyChanged;

            AddProperty(new RequiredStringProperty("Host:", "smtpHost"));
            AddProperty(new NumericProperty<ushort>("Port:", "port", 25));
            AddProperty(auth);
            AddProperty(new BooleanPropertyBase("Enable SSL:", "enableSsl", false));

            mUsernameIndex = Properties.Count;
            AddRemoveBasedOnMode(auth.SelectedValue, mUsernameIndex, mUsername);

            mPasswordIndex = mUsernameIndex + 1;
            AddRemoveBasedOnMode(auth.SelectedValue, mPasswordIndex, mPassword);

            AddProperty(new RequiredStringProperty("To:", "to"));
            AddProperty(new RequiredStringProperty("From:", "from"));
            AddProperty(new StringValueProperty("Reply To:", "replyTo"));
            AddProperty(new StringValueProperty("Cc:", "cc"));
            AddProperty(new StringValueProperty("Bcc:", "bcc"));
            AddProperty(new StringValueProperty("Subject:", "subject"));
            AddProperty(new Encoding("Subject Encoding:", "subjectEncoding"));
            AddProperty(new Encoding("Body Encoding:", "bodyEncoding"));
            AddProperty(new EnumProperty<MailPriority>("Priority:", 75, "priority"));
        }

        private void AuthenticationOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EnumProperty<SmtpAuthentication>.SelectedValue))
            {
                AddRemoveBasedOnMode(((EnumProperty<SmtpAuthentication>)sender).SelectedValue, mUsernameIndex, mUsername);
                AddRemoveBasedOnMode(((EnumProperty<SmtpAuthentication>)sender).SelectedValue, mPasswordIndex, mPassword);
            }
        }

        private void AddRemoveBasedOnMode(string selectedMode, int index, IProperty appenderProperty)
        {
            if (selectedMode == SmtpAuthentication.Basic.ToString())
            {
                if (!Properties.Contains(appenderProperty))
                {
                    AddProperty(index, appenderProperty);
                }
            }
            else
            {
                RemoveProperty(appenderProperty);
            }
        }
    }
}

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
        private int mUsernameIndex;
        private readonly RequiredStringProperty mUsername;

        private int mPasswordIndex;
        private readonly RequiredStringProperty mPassword;

        public SmtpAppender(IElementConfiguration configuration)
            : base(configuration, true)
        {
            mUsername = new RequiredStringProperty(Properties, "Username:", "username");
            mPassword = new RequiredStringProperty(Properties, "Password:", "password");
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Smtp;

        public override string Name => "SMTP Appender";

        protected override void AddAppenderSpecificProperties()
        {
            base.AddAppenderSpecificProperties();

            EnumProperty<SmtpAuthentication> auth = new EnumProperty<SmtpAuthentication>(Properties, "Authentication:", 60, "authentication");
            auth.PropertyChanged += AuthenticationOnPropertyChanged;

            AddProperty(new RequiredStringProperty(Properties, "Host:", "smtpHost"));
            AddProperty(new Port(Properties, "Port:", "port"));
            AddProperty(auth);
            AddProperty(new BooleanPropertyBase(Properties, "Enable SSL:", "enableSsl", false));

            mUsernameIndex = Properties.Count;
            AddRemoveBasedOnMode(auth.SelectedValue, mUsernameIndex, mUsername);

            mPasswordIndex = mUsernameIndex + 1;
            AddRemoveBasedOnMode(auth.SelectedValue, mPasswordIndex, mPassword); //TODO obfuscate?

            AddProperty(new RequiredStringProperty(Properties, "To:", "to"));
            AddProperty(new RequiredStringProperty(Properties, "From:", "from"));
            AddProperty(new StringValueProperty(Properties, "Reply To:", "replyTo"));
            AddProperty(new StringValueProperty(Properties, "Cc:", "cc"));
            AddProperty(new StringValueProperty(Properties, "Bcc:", "bcc"));
            AddProperty(new StringValueProperty(Properties, "Subject:", "subject"));
            AddProperty(new Encoding(Properties, "Subject Encoding:", "subjectEncoding"));
            AddProperty(new Encoding(Properties, "Body Encoding:", "bodyEncoding"));
            AddProperty(new EnumProperty<MailPriority>(Properties, "Priority:", 75, "priority"));
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

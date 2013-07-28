using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Xml.Linq;

namespace SquidEyes.Generic
{
    public class IdentityInfo
    {
        internal IdentityInfo()
        {
            UserName = WindowsIdentity.GetCurrent().Name;
            Authentication = WindowsIdentity.GetCurrent().AuthenticationType;
            IsAuthenticated = WindowsIdentity.GetCurrent().IsAuthenticated;
            Impersonation = WindowsIdentity.GetCurrent().ImpersonationLevel;
            IsAnonymous = WindowsIdentity.GetCurrent().IsAnonymous;
            IsGuest = WindowsIdentity.GetCurrent().IsGuest;
            IsSystem = WindowsIdentity.GetCurrent().IsSystem;

            Groups = new List<string>();

            foreach (IdentityReference group in WindowsIdentity.GetCurrent().Groups)
            {
                string groupName;

                try
                {
                    groupName = ((NTAccount)group.
                        Translate(typeof(NTAccount))).Value;
                }
                catch
                {
                    groupName = group.Value;
                }

                Groups.Add(groupName);
            }

            Groups.Sort();
        }

        internal IdentityInfo(XElement system)
        {
            var identity = system.Element("identity");

            UserName = (string)identity.Element("userName");
            Authentication = (string)identity.Element("authentication");
            IsAuthenticated = (bool)identity.Element("isAuthenticated");
            Impersonation = ((string)identity.Element("impersonation")).
                ToEnum<TokenImpersonationLevel>();
            IsAnonymous = (bool)identity.Element("isAnonymous");
            IsGuest = (bool)identity.Element("isGuest");
            IsSystem = (bool)identity.Element("isSystem");

            Groups = new List<string>();

            foreach(var group in identity.Element("groups").Elements("group"))
                Groups.Add(group.Value);
        }

        public string UserName { get; private set; }
        public List<string> Groups { get; private set; }
        public string Authentication { get; private set; }
        public bool IsAuthenticated { get; private set; }
        public TokenImpersonationLevel Impersonation { get; private set; }
        public bool IsAnonymous { get; private set; }
        public bool IsGuest { get; private set; }
        public bool IsSystem { get; private set; }

        internal XElement GetElement()
        {
            return new XElement("identity",
                new XElement("userName", UserName),
                new XElement("authentication", Authentication),
                new XElement("isAuthenticated", IsAuthenticated),
                new XElement("impersonation", Impersonation),
                new XElement("isAnonymous", IsAnonymous),
                new XElement("isGuest", IsGuest),
                new XElement("isSystem", IsSystem),
                new XElement("groups",
                    from g in Groups
                    select new XElement("group", g)));
        }
    }
}
﻿using CertificateManager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using WCFCommons;

namespace SecureHost
{
    public class SecureHost
    {

        public SecureHostConfig HostConfig { get; private set; }

        private ServiceHost serviceHost;

        public SecureHost(SecureHostConfig hostConfig)
        {
            HostConfig = hostConfig.Clone();
        }

        public string GetHostAddress()
        {
            return String.Format("net.tcp://{0}:{1}/{2}", HostConfig.Ip, HostConfig.Port, "ICommonService");
        }

        public void Open()
        {
            switch (HostConfig.AuthenticationType)
            {
                case EAuthType.Windows:
                    OpenWinAuthService();
                    break;
                case EAuthType.Cert:
                    OpenCertAuthService();
                    break;
            }
        }

        private void OpenWinAuthService()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;


            serviceHost = new ServiceHost(typeof(WinAuthCommonService));
            serviceHost.AddServiceEndpoint(typeof(ICommonService), binding, GetHostAddress());

            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>()
            {
                new WinRBACAuthorizationPolicy()
            };

            //serviceHost.Authorization.ServiceAuthorizationManager = new MyAuthorizationManager();
            serviceHost.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();
            serviceHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;

            serviceHost.Open();
        }

        private void OpenCertAuthService()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            //binding.Security.Transport.

            serviceHost = new ServiceHost(typeof(WinAuthCommonService));
            serviceHost.AddServiceEndpoint(typeof(ICommonService), binding, GetHostAddress());

            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>()
            {
                new WinRBACAuthorizationPolicy()
            };

            serviceHost.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;

            serviceHost.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            serviceHost.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "wcfservice");

            serviceHost.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();
            serviceHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;

            // serviceHost.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "wcfclient");

            //serviceHost.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();
            //serviceHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;



            serviceHost.Open();
        }


    }
}

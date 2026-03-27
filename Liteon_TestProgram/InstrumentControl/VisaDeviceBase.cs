using Ivi.Visa;
using Liteon_TestProgram.Utilities;
using NationalInstruments.Visa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.InstrumentControl
{

    internal abstract class VisaDeviceBase : IDisposable
    {
        public abstract void OnConfiguration(VisaConfigurationBuilder configurationBuilders);
        public MessageBasedSession Session;
        private string _name;
        private string _resourceName = string.Empty;
        private bool _disposed;

        internal byte[] readBuffer = new byte[1024];
        public string ResourceName { get => _resourceName; }

        protected bool _isConnected;
        public bool IsConnected => _isConnected;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            Session?.Dispose();
            Session = null;
            _isConnected = false;
            _disposed = true;
        }


        public VisaDeviceBase( string str_Addr)
        {
            _name = GetType().Name;
            _resourceName = str_Addr;
            Connect(_resourceName);
        }

        protected void Write(string command)
        {
            if (!_isConnected || Session is null)
            {
                throw new InvalidOperationException("Session not initialized!");
            }

            try
            {
                Session.RawIO.Write(command);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("write operation has failed", ex);
            }
        }

        protected string Query(string command)
        {
            if (!_isConnected || Session is null)
            {
                throw new InvalidOperationException("Session not initialized!");
            }

            try
            {
                Session.RawIO.Write(command);
                string response = Session.RawIO.ReadString();
                return ReplaceCommonEscapeSequences(response);
            }
            catch (Exception ex) when (ex.InnerException is VisaException)
            {
                throw new ArgumentException("write operation has failed", ex);
            }
        }

        protected void WriteLine(string command) => Write($"{command}\n");


        protected string Read()
        {
            if (!_isConnected || Session is null)
            {
                throw new InvalidOperationException("Session not initialized!");
            }


            try
            {
                Session.RawIO.Read(readBuffer, 0, readBuffer.LongLength, out var count, out _);
                string response = Encoding.ASCII.GetString(readBuffer, 0, (int)count);
                return ReplaceCommonEscapeSequences(response);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("read failed", ex);
            }

        }


        public void Connect(string resourceName)
        {
            _resourceName = resourceName;
            Connect();
        }

        public virtual void Connect()
        {
            UIHandleHelper.ShowRunLog($"|INFO|Visa| start connect to {_name} {ResourceName}");
            if (_isConnected)
            {
                UIHandleHelper.ShowRunLog($"|INFO|Visa| {_name} already connected");
                return;
            }

            if (string.IsNullOrEmpty(_resourceName))
            {
                throw new ArgumentException(nameof(_resourceName));
            }

            Session?.Dispose();

            try
            {
                using ResourceManager resourceManager = new();

                Session = (MessageBasedSession)resourceManager.Open(ResourceName);
            }
            catch (Exception ex)
            {
                string errorInfo = ex switch
                {
                    VisaException => $"Resource Name not found! {ResourceName}; visa message:{ex.Message}",
                    TargetInvocationException => "dependency missing",
                    InvalidCastException => $"resource selected must be a message-based session! {ResourceName}",
                    _ => "see inner information",
                };
                UIHandleHelper.ShowRunLog($"|FAIL|Visa| Error {errorInfo}");
                throw new ArgumentException(errorInfo, ex);
            }

            VisaConfigurationBuilder configurationBuilder = new(this);
            _isConnected = true;
            OnConfiguration(configurationBuilder);
            UIHandleHelper.ShowRunLog($"|INFO|Visa| {_name} {ResourceName} connect OK");
        }

        private static string ReplaceCommonEscapeSequences(string s)
        {
            return s.Replace("\\n", "\n").Replace("\\r", "\r");
        }

    }
}

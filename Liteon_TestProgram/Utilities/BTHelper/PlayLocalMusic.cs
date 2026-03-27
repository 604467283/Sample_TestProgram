using Liteon_TestProgram.Forms;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.BTHelper
{
    internal class PlayLocalMusic : IDisposable
    {
        private WaveOutEvent _waveOut;
        private bool _disposed = false;

        public bool PlayMusic(string audioFilePath)
        {
            try
            {
                _waveOut = new WaveOutEvent();
                var audioFile = new AudioFileReader(audioFilePath);
                _waveOut.Init(audioFile);

                Thread.Sleep(1000);

                _waveOut.Play();

                var ret = MessageBoxEX.Show("请确认耳机是否播放音乐", false);

                if (ret == DialogResult.Yes)
                {
                    UIHandleHelper.ShowRunLog("测试人员选择了YES，播放音乐正常");
                    return true;
                }
                else
                {
                    UIHandleHelper.ShowRunLog("测试人员选择了NO，播放音乐不正常", true);
                }

                return false;
            }
            catch (Exception ex)
            {
                UIHandleHelper.ShowRunLog($"播放音乐出错{ex}", true);
                return false;
            }
            finally
            {
                StopAndDisconnect();
            }
        }

        private void StopAndDisconnect()
        {
            try
            {
                if (_waveOut != null)
                {
                    _waveOut?.Stop();
                    _waveOut?.Dispose();
                    _waveOut = null;
                }

                Console.WriteLine("已断开连接");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"断开连接时发生错误: {ex.Message}");
            }
        }

        // IDisposable implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    StopAndDisconnect();
                }
                _disposed = true;
            }
        }

        ~PlayLocalMusic()
        {
            Dispose(false);
        }
    }
}

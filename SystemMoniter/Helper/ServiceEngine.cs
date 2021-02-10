using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace SystemMoniter.Helper
{
    public class ServiceEngine
    {
        public static string serviceName = "ServiceName";

        public string ServiceStatus()
        {
            try
            {
                ServiceController sc = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName.Equals(serviceName));
                if (sc != null)
                {
                    return sc.Status.ToString();
                }
                else
                {
                    return "Service not found";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string StartService()
        {
            try
            {
                ServiceController sc = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName.Equals(serviceName));
                if (sc != null)
                {
                    if (sc.Status != ServiceControllerStatus.Running)
                    {
                        sc.Start();
                        return "Started";
                    }
                    else
                    {
                        return "Service already running";
                    }
                }
                else
                {
                    return "Service not found";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ContinueService()
        {
            try
            {
                ServiceController sc = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName.Equals(serviceName));
                if (sc != null)
                {
                    if (sc.Status == ServiceControllerStatus.Paused)
                    {
                        sc.Continue();
                        return "Resumed";
                    }
                    else
                    {
                        return "Service not paused";
                    }
                }
                else
                {
                    return "Service not found";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string StopService()
        {
            try
            {
                ServiceController sc = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName.Equals(serviceName));
                try
                {
                    if (sc != null)
                    {
                        if (sc.Status != ServiceControllerStatus.Stopped)
                        {
                            sc.Stop();
                            return "Stopped";
                        }
                        else
                        {
                            return "Service already stopped";
                        }
                    }
                    else
                    {
                        return "Service not found";
                    }
                }
                catch (Exception ex)
                {
                    Process[] procs = Process.GetProcessesByName(sc.ServiceName);
                    if (procs.Length > 0)
                    {
                        foreach (Process proc in procs)
                        {
                            using (proc)
                            {
                                try
                                {
                                    //Try to kill the service process
                                    proc.Kill();
                                }
                                catch
                                {
                                    //Try to terminate the service using taskkill command
                                    try
                                    {
                                        Process.Start(new ProcessStartInfo
                                        {
                                            FileName = "cmd.exe",
                                            CreateNoWindow = true,
                                            UseShellExecute = false,
                                            Arguments = string.Format("/c taskkill /pid {0} /f", proc.Id)
                                        });
                                    }
                                    catch (Exception ex2)
                                    {
                                        return ex2.Message;
                                    }
                                }
                            }
                        }
                        return "Stopped";
                    }
                    return ex.Message;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string RestartService()
        {
            try
            {
                ServiceController sc = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName.Equals(serviceName));
                try
                {
                    if (sc != null)
                    {
                        if (sc.Status != ServiceControllerStatus.Stopped)
                        {
                            sc.Stop();
                            Thread.Sleep(3000);
                            sc.Refresh();
                            sc.Start();
                        }
                        else
                        {
                            sc.Start();
                        }
                        return "Restarted";
                    }
                    else
                    {
                        return "Service not found";
                    }
                }
                catch (Exception ex)
                {
                    Process[] procs = Process.GetProcessesByName(sc.ServiceName);
                    if (procs.Length > 0)
                    {
                        foreach (Process proc in procs)
                        {
                            using (proc)
                            {
                                try
                                {
                                    //Try to kill the service process
                                    proc.Kill();
                                }
                                catch
                                {
                                    //Try to terminate the service using taskkill command
                                    try
                                    {
                                        Process.Start(new ProcessStartInfo
                                        {
                                            FileName = "cmd.exe",
                                            CreateNoWindow = true,
                                            UseShellExecute = false,
                                            Arguments = string.Format("/c taskkill /pid {0} /f", proc.Id)
                                        });
                                    }
                                    catch (Exception ex2)
                                    {
                                        return ex.Message + ", " + ex2.Message;
                                    }
                                }
                            }
                        }

                        sc.Refresh();
                        Thread.Sleep(3000);
                        sc.Start();
                        return "Restarted";
                    }
                    return ex.Message;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string PauseService()
        {
            try
            {
                ServiceController sc = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName.Equals(serviceName));
                if (sc != null)
                {
                    sc.Pause();
                    return "Paused";
                }
                else
                {
                    return "Service not found";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    public enum SimpleServiceCustomCommands
    { StopWorker = 128, RestartWorker, CheckWorker };
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CSnakes.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Modern
{
    public class CSnakesInitialization
    {
        private static IPythonEnvironment? _env;
        public static IDemo? _module;

        public static void InitializeSnakes()
        {
            if (_module != null)
                return;

            string venvPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".venv");

            
 
            var builder = Host.CreateApplicationBuilder();
            var home = Path.Join(Environment.CurrentDirectory, ".");

            builder.Services
                .WithPython()
                .WithHome(home)
                .WithVirtualEnvironment(venvPath)
                .WithPipInstaller()
                .FromRedistributable();

            var app = builder.Build();
            var env = app.Services.GetRequiredService<IPythonEnvironment>();

              
            

            _env = env;

           
            _module = env.Demo();
            
        }

        

       public static void InstallPyTorchWithCuda()
        {
            var venvPath = Path.Join(Environment.CurrentDirectory, ".", ".venv");
            var pipPath = OperatingSystem.IsWindows()
                ? Path.Join(venvPath, "Scripts", "pip.exe")
                : Path.Join(venvPath, "bin", "pip3");

            if (!File.Exists(pipPath))
            {
               
                return;
            }

            
             RunPipCommand(pipPath, "uninstall torch torchvision torchaudio -y", showOutput: false);


            bool success = RunPipCommand(
                pipPath,
                "install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu126 --force-reinstall --no-cache-dir",
                showOutput: true
            );

        }

        public static bool VerifyCudaAvailability()
        {
            var venvPath = Path.Join(Environment.CurrentDirectory, ".", ".venv");
            var pythonPath = OperatingSystem.IsWindows()
                ? Path.Join(venvPath, "Scripts", "python.exe")
                : Path.Join(venvPath, "bin", "python3");

            if (!File.Exists(pythonPath))
            {
               
                return false;
            }

           

            var verifyProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = pythonPath,
                    Arguments = "-c \"import torch; print(f'PyTorch version: {torch.__version__}'); print(f'CUDA available: {torch.cuda.is_available()}'); print(f'CUDA version: {torch.version.cuda if torch.cuda.is_available() else \\\"N/A\\\"}'); print(f'GPU count: {torch.cuda.device_count()}'); print(f'GPU name: {torch.cuda.get_device_name(0) if torch.cuda.is_available() else \\\"N/A\\\"}')\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            verifyProcess.Start();
            string output = verifyProcess.StandardOutput.ReadToEnd();
            string error = verifyProcess.StandardError.ReadToEnd();
            verifyProcess.WaitForExit();

            Console.WriteLine(output);

            if (output.Contains("CUDA available: True"))
            {
               
                return true;
            }
            else
            {
                
               return false;
            }

            
        }

        static bool RunPipCommand(string pipPath, string arguments, bool showOutput = false)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = pipPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            if (showOutput)
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        Console.WriteLine($"  {e.Data}");
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        Console.WriteLine($"  {e.Data}");
                };
            }

            process.Start();

            if (showOutput)
            {
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }

            process.WaitForExit();

            return process.ExitCode == 0;
        }

    }
}

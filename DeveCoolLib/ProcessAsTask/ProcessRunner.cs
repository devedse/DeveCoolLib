﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DeveCoolLib.ProcessAsTask
{
    public static class ProcessRunner
    {
        /// <summary>
        /// Runs asynchronous process.
        /// </summary>
        /// <param name="processStartInfo">The <see cref="T:System.Diagnostics.ProcessStartInfo" /> that contains the information that is used to start the process, including the file name and any command-line arguments.</param>
        /// <param name="standardOutput">List that lines written to standard output by the process will be added to</param>
        /// <param name="standardError">List that lines written to standard error by the process will be added to</param>
        /// <param name="forwardLogsToConsole">Forwards the process logs to the console</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public static async Task<ProcessResults> RunAsync(ProcessStartInfo processStartInfo, List<string> standardOutput, List<string> standardError, CancellationToken cancellationToken, bool forwardLogsToConsole)
        {
            // force some settings in the start info so we can capture the output
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            var tcs = new TaskCompletionSource<ProcessResults>();

            using (var process = new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            })
            {

                var standardOutputResults = new TaskCompletionSource<string[]>();
                process.OutputDataReceived += (sender, args) =>
                {
                    if (args.Data != null)
                    {
                        if (forwardLogsToConsole)
                        {
                            Console.WriteLine(args.Data);
                        }
                        standardOutput.Add(args.Data);
                    }
                    else
                    {
                        standardOutputResults.SetResult(standardOutput.ToArray());
                    }
                };

                var standardErrorResults = new TaskCompletionSource<string[]>();
                process.ErrorDataReceived += (sender, args) =>
                {
                    if (args.Data != null)
                    {
                        if (forwardLogsToConsole)
                        {
                            Console.Error.WriteLine(args.Data);
                        }
                        standardError.Add(args.Data);
                    }
                    else
                    {
                        standardErrorResults.SetResult(standardError.ToArray());
                    }
                };

                var processStartTime = new TaskCompletionSource<DateTime>();

                process.Exited += async (sender, args) =>
                {
                    // Since the Exited event can happen asynchronously to the output and error events, 
                    // we await the task results for stdout/stderr to ensure they both closed.  We must await
                    // the stdout/stderr tasks instead of just accessing the Result property due to behavior on MacOS.  
                    // For more details, see the PR at https://github.com/jamesmanning/RunProcessAsTask/pull/16/
                    tcs.TrySetResult(
                            new ProcessResults(
                                process,
                                await processStartTime.Task.ConfigureAwait(false),
                                await standardOutputResults.Task.ConfigureAwait(false),
                                await standardErrorResults.Task.ConfigureAwait(false)
                            )
                        );
                };

                using (cancellationToken.Register(
                    () =>
                    {
                        tcs.TrySetCanceled();
                        try
                        {
                            if (!process.HasExited)
                            {
                                process.Kill();
                            }
                        }
                        catch (InvalidOperationException) { }
                    }))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var startTime = DateTime.Now;
                    if (process.Start() == false)
                    {
                        tcs.TrySetException(new InvalidOperationException("Failed to start process"));
                    }
                    else
                    {
                        try
                        {
                            startTime = process.StartTime;
                        }
                        catch (Exception)
                        {
                            // best effort to try and get a more accurate start time, but if we fail to access StartTime
                            // (for instance, process has already existed), we still have a valid value to use.
                        }
                        processStartTime.SetResult(startTime);

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                    }

                    return await tcs.Task.ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Runs asynchronous process.
        /// </summary>
        /// <param name="fileName">An application or document which starts the process.</param>
        /// <param name="forwardLogsToConsole">Forwards the process logs to the console</param>
        public static Task<ProcessResults> RunAsync(string fileName, bool forwardLogsToConsole = false)
            => RunAsync(new ProcessStartInfo(fileName), forwardLogsToConsole);

        /// <summary>
        /// Runs asynchronous process.
        /// </summary>
        /// <param name="fileName">An application or document which starts the process.</param>
        /// <param name="forwardLogsToConsole">Forwards the process logs to the console</param>
        public static Task<ProcessResults> RunAsync(string fileName, IDictionary<string, string> environmentVariables, bool forwardLogsToConsole = false)
            => RunAsync(fileName, environmentVariables, CancellationToken.None, forwardLogsToConsole);

        /// <summary>
        /// Runs asynchronous process.
        /// </summary>
        /// <param name="fileName">An application or document which starts the process.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <param name="forwardLogsToConsole">Forwards the process logs to the console</param>
        public static Task<ProcessResults> RunAsync(string fileName, CancellationToken cancellationToken, bool forwardLogsToConsole = false)
            => RunAsync(new ProcessStartInfo(fileName), cancellationToken, forwardLogsToConsole);

        /// <summary>
        /// Runs asynchronous process.
        /// </summary>
        /// <param name="fileName">An application or document which starts the process.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <param name="forwardLogsToConsole">Forwards the process logs to the console</param>
        public static Task<ProcessResults> RunAsync(string fileName, IDictionary<string, string> environmentVariables, CancellationToken cancellationToken, bool forwardLogsToConsole = false)
        {
            var psi = new ProcessStartInfo(fileName);
            foreach (var envVariable in environmentVariables)
            {
                psi.EnvironmentVariables.Add(envVariable.Key, envVariable.Value);
            }

            return RunAsync(psi, cancellationToken, forwardLogsToConsole);
        }

        /// <summary>
        /// Runs asynchronous process.
        /// </summary>
        /// <param name="fileName">An application or document which starts the process.</param>
        /// <param name="arguments">Command-line arguments to pass to the application when the process starts.</param>
        /// <param name="forwardLogsToConsole">Forwards the process logs to the console</param>
        public static Task<ProcessResults> RunAsync(string fileName, string arguments, bool forwardLogsToConsole = false)
            => RunAsync(new ProcessStartInfo(fileName, arguments), forwardLogsToConsole);

        /// <summary>
        /// Runs asynchronous process.
        /// </summary>
        /// <param name="fileName">An application or document which starts the process.</param>
        /// <param name="arguments">Command-line arguments to pass to the application when the process starts.</param>
        /// <param name="environmentVariables">Environment variables to use when the process starts.</param>
        /// <param name="forwardLogsToConsole">Forwards the process logs to the console</param>
        public static Task<ProcessResults> RunAsync(string fileName, string arguments, IDictionary<string, string> environmentVariables, bool forwardLogsToConsole = false)
            => RunAsync(fileName, arguments, environmentVariables, CancellationToken.None, forwardLogsToConsole);

        /// <summary>
        /// Runs asynchronous process.
        /// </summary>
        /// <param name="fileName">An application or document which starts the process.</param>
        /// <param name="arguments">Command-line arguments to pass to the application when the process starts.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <param name="forwardLogsToConsole">Forwards the process logs to the console</param>
        public static Task<ProcessResults> RunAsync(string fileName, string arguments, CancellationToken cancellationToken, bool forwardLogsToConsole = false)
            => RunAsync(new ProcessStartInfo(fileName, arguments), cancellationToken, forwardLogsToConsole);

        /// <summary>
        /// Runs asynchronous process.
        /// </summary>
        /// <param name="fileName">An application or document which starts the process.</param>
        /// <param name="arguments">Command-line arguments to pass to the application when the process starts.</param>
        /// <param name="environmentVariables">Environment variables to use when the process starts.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <param name="forwardLogsToConsole">Forwards the process logs to the console</param>
        public static Task<ProcessResults> RunAsync(string fileName, string arguments, IDictionary<string, string> environmentVariables, CancellationToken cancellationToken, bool forwardLogsToConsole = false)
        {
            var psi = new ProcessStartInfo(fileName, arguments);
            foreach (var envVariable in environmentVariables)
            {
                psi.EnvironmentVariables.Add(envVariable.Key, envVariable.Value);
            }

            return RunAsync(psi, cancellationToken, forwardLogsToConsole);
        }

        /// <summary>
        /// Runs asynchronous process.
        /// </summary>
        /// <param name="processStartInfo">The <see cref="T:System.Diagnostics.ProcessStartInfo" /> that contains the information that is used to start the process, including the file name and any command-line arguments.</param>
        /// <param name="forwardLogsToConsole">Forwards the process logs to the console</param>
        public static Task<ProcessResults> RunAsync(ProcessStartInfo processStartInfo, bool forwardLogsToConsole = false)
            => RunAsync(processStartInfo, CancellationToken.None, forwardLogsToConsole);

        /// <summary>
        /// Runs asynchronous process.
        /// </summary>
        /// <param name="processStartInfo">The <see cref="T:System.Diagnostics.ProcessStartInfo" /> that contains the information that is used to start the process, including the file name and any command-line arguments.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <param name="forwardLogsToConsole">Forwards the process logs to the console</param>
        public static Task<ProcessResults> RunAsync(ProcessStartInfo processStartInfo, CancellationToken cancellationToken, bool forwardLogsToConsole = false)
            => RunAsync(processStartInfo, new List<string>(), new List<string>(), cancellationToken, forwardLogsToConsole);
    }
}


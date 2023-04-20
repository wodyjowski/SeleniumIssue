# Selenium issue example
Affects `selenium-hub > 4.5.0`

https://github.com/SeleniumHQ/selenium/issues/11408  
https://github.com/microsoft/playwright/issues/18892  

## Requirements
* .NET 7
* docker

## Usage
Run `TestSeleniumScriptExecution` test.

## Description

>Since 4.5.2 (i.e. JDK11 client), we've been experiencing an issue with the WebSocket. In this case, large messages sent over the WebSocket proxy (i.e. Selenium Hub + Selenium Node configuration), the receiver incorrectly assumes that the received frame is final. This results in a JSON decoding error in the string builder, which in turn results in the socket closing.  
<br/>Notably, in the logs of the Chrome Node, there is first a "Frame is not final" message, but then we see that the WebSocket assumes that it was actually a final part:  
<br/>Final part of text message received. Calling listener with
{"error":{"code":-32700,"message":"JSON: string literal expected at position 16383"}}
Duhh, it's not possible to decode the JSON when you're missing the rest of the message..

*Source: https://github.com/SeleniumHQ/selenium/issues/11408#issue-1488598072*

Selenium hub on port **4444** uses version `4.5.0`, second one on port **4445** uses version `4.8.3`
Example was made using official `Selenium.WebDriver` NuGet package.

Output from problematic grid instance:

```
2023-04-21 00:14:01 22:14:01.939 INFO [LocalDistributor.newSession] - Session request received by the Distributor: 
2023-04-21 00:14:01  [Capabilities {browserName: chrome, goog:chromeOptions: {}}]
2023-04-21 00:14:02 22:14:02.334 INFO [LocalDistributor.newSession] - Session created by the Distributor. Id: 557b4033cce9bbfb84a249c6a44d631f 
2023-04-21 00:14:02  Caps: Capabilities {acceptInsecureCerts: false, browserName: chrome, browserVersion: 111.0.5563.146, chrome: {chromedriverVersion: 111.0.5563.64 (c710e93d5b63..., userDataDir: /tmp/.com.google.Chrome.ZEZeNW}, goog:chromeOptions: {debuggerAddress: localhost:33455}, networkConnectionEnabled: false, pageLoadStrategy: normal, platformName: LINUX, proxy: {}, se:bidiEnabled: false, se:cdp: ws://localhost:4445/session..., se:cdpVersion: 111.0.5563.146, se:vnc: ws://localhost:4445/session..., se:vncEnabled: true, se:vncLocalAddress: ws://172.21.0.4:7900, setWindowRect: true, strictFileInteractability: false, timeouts: {implicit: 0, pageLoad: 300000, script: 30000}, unhandledPromptBehavior: dismiss and notify, webauthn:extension:credBlob: true, webauthn:extension:largeBlob: true, webauthn:extension:minPinLength: true, webauthn:extension:prf: true, webauthn:virtualAuthenticators: true}
2023-04-21 00:14:03 22:14:03.630 WARN [ProxyWebsocketsIntoGrid$ForwardingListener.onError] - Error proxying websocket command
2023-04-21 00:14:03 java.net.ProtocolException
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.WebSocketImpl$ReceiveTask.processError(WebSocketImpl.java:487)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.WebSocketImpl$ReceiveTask.run(WebSocketImpl.java:454)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler$CompleteRestartableTask.run(SequentialScheduler.java:147)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler$SchedulableTask.run(SequentialScheduler.java:198)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:271)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:224)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.WebSocketImpl.trySetState(WebSocketImpl.java:837)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.WebSocketImpl.signalError(WebSocketImpl.java:714)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.WebSocketImpl$SignallingMessageConsumer.onError(WebSocketImpl.java:825)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.TransportImpl$ReceiveTask.run(TransportImpl.java:671)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler$CompleteRestartableTask.run(SequentialScheduler.java:147)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler$SchedulableTask.run(SequentialScheduler.java:198)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:271)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:224)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.TransportImpl$ReadEvent.handle(TransportImpl.java:762)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.RawChannelTube$ReadSubscriber.checkEvents(RawChannelTube.java:174)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.RawChannelTube$ReadSubscriber.onNext(RawChannelTube.java:204)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.RawChannelTube$ReadSubscriber.onNext(RawChannelTube.java:157)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.SocketTube$InternalReadPublisher$InternalReadSubscription.read(SocketTube.java:844)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.SocketTube$SocketFlowTask.run(SocketTube.java:175)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler$SchedulableTask.run(SequentialScheduler.java:198)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:271)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:224)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.SocketTube$InternalReadPublisher$InternalReadSubscription.signalReadable(SocketTube.java:763)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.SocketTube$InternalReadPublisher$ReadEvent.signalEvent(SocketTube.java:941)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.SocketTube$SocketFlowEvent.handle(SocketTube.java:245)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.HttpClientImpl$SelectorManager.handleEvent(HttpClientImpl.java:957)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.HttpClientImpl$SelectorManager.lambda$run$3(HttpClientImpl.java:912)
2023-04-21 00:14:03 at java.base/java.util.ArrayList.forEach(ArrayList.java:1541)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.HttpClientImpl$SelectorManager.run(HttpClientImpl.java:912)
2023-04-21 00:14:03 Caused by: jdk.internal.net.http.websocket.FailWebSocketException: Unexpected frame CONTINUATION (fin=false)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.MessageDecoder.opcode(MessageDecoder.java:133)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.Frame$Reader.readFrame(Frame.java:391)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.TransportImpl$ReceiveTask.run(TransportImpl.java:665)
2023-04-21 00:14:03 ... 20 more
2023-04-21 00:14:03 22:14:03.632 WARN [DefaultChannelPipeline.onUnhandledInboundException] - An exceptionCaught() event was fired, and it reached at the tail of the pipeline. It usually means the last handler in the pipeline did not handle the exception.
2023-04-21 00:14:03 org.openqa.selenium.WebDriverException: java.io.IOException: Output closed
2023-04-21 00:14:03 Build info: version: '4.8.3', revision: 'b19b418e60'
2023-04-21 00:14:03 System info: os.name: 'Linux', os.arch: 'amd64', os.version: '5.10.102.1-microsoft-standard-WSL2', java.version: '11.0.18'
2023-04-21 00:14:03 Driver info: driver.version: unknown
2023-04-21 00:14:03 at org.openqa.selenium.remote.http.jdk.JdkHttpClient$5.send(JdkHttpClient.java:239)
2023-04-21 00:14:03 at org.openqa.selenium.netty.server.WebSocketMessageHandler.lambda$channelRead0$0(WebSocketMessageHandler.java:47)
2023-04-21 00:14:03 at io.netty.util.concurrent.AbstractEventExecutor.runTask(AbstractEventExecutor.java:174)
2023-04-21 00:14:03 at io.netty.util.concurrent.AbstractEventExecutor.safeExecute(AbstractEventExecutor.java:167)
2023-04-21 00:14:03 at io.netty.util.concurrent.SingleThreadEventExecutor.runAllTasks(SingleThreadEventExecutor.java:470)
2023-04-21 00:14:03 at io.netty.channel.nio.NioEventLoop.run(NioEventLoop.java:566)
2023-04-21 00:14:03 at io.netty.util.concurrent.SingleThreadEventExecutor$4.run(SingleThreadEventExecutor.java:997)
2023-04-21 00:14:03 at io.netty.util.internal.ThreadExecutorMap$2.run(ThreadExecutorMap.java:74)
2023-04-21 00:14:03 at io.netty.util.concurrent.FastThreadLocalRunnable.run(FastThreadLocalRunnable.java:30)
2023-04-21 00:14:03 at java.base/java.lang.Thread.run(Thread.java:829)
2023-04-21 00:14:03 Caused by: java.io.IOException: Output closed
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.TransportImpl$SendTask.tryCompleteWrite(TransportImpl.java:609)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.TransportImpl$SendTask.run(TransportImpl.java:533)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler$CompleteRestartableTask.run(SequentialScheduler.java:147)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler$SchedulableTask.run(SequentialScheduler.java:198)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:271)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:224)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.TransportImpl.closeOutput(TransportImpl.java:318)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.WebSocketImpl.close(WebSocketImpl.java:736)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.WebSocketImpl.signalError(WebSocketImpl.java:720)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.WebSocketImpl$SignallingMessageConsumer.onError(WebSocketImpl.java:825)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.TransportImpl$ReceiveTask.run(TransportImpl.java:671)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler$CompleteRestartableTask.run(SequentialScheduler.java:147)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler$SchedulableTask.run(SequentialScheduler.java:198)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:271)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:224)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.websocket.TransportImpl$ReadEvent.handle(TransportImpl.java:762)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.RawChannelTube$ReadSubscriber.checkEvents(RawChannelTube.java:174)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.RawChannelTube$ReadSubscriber.onNext(RawChannelTube.java:204)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.RawChannelTube$ReadSubscriber.onNext(RawChannelTube.java:157)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.SocketTube$InternalReadPublisher$InternalReadSubscription.read(SocketTube.java:844)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.SocketTube$SocketFlowTask.run(SocketTube.java:175)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler$SchedulableTask.run(SequentialScheduler.java:198)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:271)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:224)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.SocketTube$InternalReadPublisher$InternalReadSubscription.signalReadable(SocketTube.java:763)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.SocketTube$InternalReadPublisher$ReadEvent.signalEvent(SocketTube.java:941)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.SocketTube$SocketFlowEvent.handle(SocketTube.java:245)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.HttpClientImpl$SelectorManager.handleEvent(HttpClientImpl.java:957)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.HttpClientImpl$SelectorManager.lambda$run$3(HttpClientImpl.java:912)
2023-04-21 00:14:03 at java.base/java.util.ArrayList.forEach(ArrayList.java:1541)
2023-04-21 00:14:03 at java.net.http/jdk.internal.net.http.HttpClientImpl$SelectorManager.run(HttpClientImpl.java:912)
2023-04-21 00:14:08 22:14:08.678 WARN [DefaultChannelPipeline.onUnhandledInboundException] - An exceptionCaught() event was fired, and it reached at the tail of the pipeline. It usually means the last handler in the pipeline did not handle the exception.
2023-04-21 00:14:08 org.openqa.selenium.WebDriverException: java.lang.IllegalArgumentException: statusCode
2023-04-21 00:14:08 Build info: version: '4.8.3', revision: 'b19b418e60'
2023-04-21 00:14:08 System info: os.name: 'Linux', os.arch: 'amd64', os.version: '5.10.102.1-microsoft-standard-WSL2', java.version: '11.0.18'
2023-04-21 00:14:08 Driver info: driver.version: unknown
2023-04-21 00:14:08 at org.openqa.selenium.remote.http.jdk.JdkHttpClient$5.send(JdkHttpClient.java:239)
2023-04-21 00:14:08 at org.openqa.selenium.netty.server.WebSocketMessageHandler.lambda$channelRead0$0(WebSocketMessageHandler.java:47)
2023-04-21 00:14:08 at io.netty.util.concurrent.AbstractEventExecutor.runTask(AbstractEventExecutor.java:174)
2023-04-21 00:14:08 at io.netty.util.concurrent.AbstractEventExecutor.safeExecute(AbstractEventExecutor.java:167)
2023-04-21 00:14:08 at io.netty.util.concurrent.SingleThreadEventExecutor.runAllTasks(SingleThreadEventExecutor.java:470)
2023-04-21 00:14:08 at io.netty.channel.nio.NioEventLoop.run(NioEventLoop.java:566)
2023-04-21 00:14:08 at io.netty.util.concurrent.SingleThreadEventExecutor$4.run(SingleThreadEventExecutor.java:997)
2023-04-21 00:14:08 at io.netty.util.internal.ThreadExecutorMap$2.run(ThreadExecutorMap.java:74)
2023-04-21 00:14:08 at io.netty.util.concurrent.FastThreadLocalRunnable.run(FastThreadLocalRunnable.java:30)
2023-04-21 00:14:08 at java.base/java.lang.Thread.run(Thread.java:829)
2023-04-21 00:14:08 Caused by: java.lang.IllegalArgumentException: statusCode
2023-04-21 00:14:08 at java.net.http/jdk.internal.net.http.websocket.WebSocketImpl.sendClose(WebSocketImpl.java:301)
2023-04-21 00:14:08 at org.openqa.selenium.remote.http.jdk.JdkHttpClient$5.lambda$send$2(JdkHttpClient.java:222)
2023-04-21 00:14:08 at org.openqa.selenium.remote.http.jdk.JdkHttpClient$5.send(JdkHttpClient.java:229)
2023-04-21 00:14:08 ... 9 more
2023-04-21 00:14:08 22:14:08.771 INFO [LocalSessionMap.lambda$new$0] - Deleted session from local Session Map, Id: 557b4033cce9bbfb84a249c6a44d631f
2023-04-21 00:14:08 22:14:08.772 INFO [GridModel.release] - Releasing slot for session id 557b4033cce9bbfb84a249c6a44d631f

```
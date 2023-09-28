# Selenium issue example
Affects `selenium-hub > 4.5.0`

https://github.com/SeleniumHQ/selenium/issues/11408  
https://github.com/microsoft/playwright/issues/18892  

## Requirements
* .NET 7
* docker

## Usage
Run `SeleniumSocketConnection_ShouldWork` test.

## Description

>Since 4.5.2 (i.e. JDK11 client), we've been experiencing an issue with the WebSocket. In this case, large messages sent over the WebSocket proxy (i.e. Selenium Hub + Selenium Node configuration), the receiver incorrectly assumes that the received frame is final. This results in a JSON decoding error in the string builder, which in turn results in the socket closing.  
<br/>Notably, in the logs of the Chrome Node, there is first a "Frame is not final" message, but then we see that the WebSocket assumes that it was actually a final part:  
<br/>Final part of text message received. Calling listener with
{"error":{"code":-32700,"message":"JSON: string literal expected at position 16383"}}
Duhh, it's not possible to decode the JSON when you're missing the rest of the message..

*Source: https://github.com/SeleniumHQ/selenium/issues/11408#issue-1488598072*

Tests spin up `Selenium Hub` with one `Node` in docker and sends WebSocket requests with multiple lengths.

Output from problematic grid instance:

```
2023-09-28 18:02:16 16:02:16.348 WARN [ProxyWebsocketsIntoGrid$ForwardingListener.onError] - Error proxying websocket command
2023-09-28 18:02:16 java.net.ProtocolException
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.websocket.WebSocketImpl$ReceiveTask.processError(WebSocketImpl.java:487)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.websocket.WebSocketImpl$ReceiveTask.run(WebSocketImpl.java:454)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.common.SequentialScheduler$CompleteRestartableTask.run(SequentialScheduler.java:147)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.common.SequentialScheduler$SchedulableTask.run(SequentialScheduler.java:198)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:271)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:224)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.websocket.WebSocketImpl.trySetState(WebSocketImpl.java:837)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.websocket.WebSocketImpl.signalError(WebSocketImpl.java:714)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.websocket.WebSocketImpl$SignallingMessageConsumer.onError(WebSocketImpl.java:825)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.websocket.TransportImpl$ReceiveTask.run(TransportImpl.java:671)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.common.SequentialScheduler$CompleteRestartableTask.run(SequentialScheduler.java:147)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.common.SequentialScheduler$SchedulableTask.run(SequentialScheduler.java:198)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:271)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:224)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.websocket.TransportImpl$ReadEvent.handle(TransportImpl.java:762)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.RawChannelTube$ReadSubscriber.checkEvents(RawChannelTube.java:174)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.RawChannelTube$ReadSubscriber.onNext(RawChannelTube.java:204)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.RawChannelTube$ReadSubscriber.onNext(RawChannelTube.java:157)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.SocketTube$InternalReadPublisher$InternalReadSubscription.read(SocketTube.java:844)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.SocketTube$SocketFlowTask.run(SocketTube.java:175)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.common.SequentialScheduler$SchedulableTask.run(SequentialScheduler.java:198)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:271)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.common.SequentialScheduler.runOrSchedule(SequentialScheduler.java:224)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.SocketTube$InternalReadPublisher$InternalReadSubscription.signalReadable(SocketTube.java:763)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.SocketTube$InternalReadPublisher$ReadEvent.signalEvent(SocketTube.java:941)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.SocketTube$SocketFlowEvent.handle(SocketTube.java:245)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.HttpClientImpl$SelectorManager.handleEvent(HttpClientImpl.java:957)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.HttpClientImpl$SelectorManager.lambda$run$3(HttpClientImpl.java:912)
2023-09-28 18:02:16     at java.base/java.util.ArrayList.forEach(ArrayList.java:1541)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.HttpClientImpl$SelectorManager.run(HttpClientImpl.java:912)
2023-09-28 18:02:16 Caused by: jdk.internal.net.http.websocket.FailWebSocketException: Unexpected frame CONTINUATION (fin=false)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.websocket.MessageDecoder.opcode(MessageDecoder.java:133)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.websocket.Frame$Reader.readFrame(Frame.java:391)
2023-09-28 18:02:16     at java.net.http/jdk.internal.net.http.websocket.TransportImpl$ReceiveTask.run(TransportImpl.java:665)
2023-09-28 18:02:16     ... 20 more
```
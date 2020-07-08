net.Error 的具体类型被封装为net.OpError

type OpError struct {
    // Op is the operation which caused the error, such as
    // "read" or "write".
    Op string

    // Net is the network type on which this error occurred,
    // such as "tcp" or "udp6".
    Net string

    // For operations involving a remote network connection, like
    // Dial, Read, or Write, Source is the corresponding local
    // network address.
    Source Addr

    // Addr is the network address for which this error occurred.
    // For local operations, like Listen or SetDeadline, Addr is
    // the address of the local endpoint being manipulated.
    // For operations involving a remote network connection, like
    // Dial, Read, or Write, Addr is the remote address of that
    // connection.
    Addr Addr
    
    // Err is the error that occurred during the operation.
    Err error
}

以下是我们可能见到的错误类型：

net.ParseError
net.AddrError
net.UnknownNetworkError
net.InvalidAddrError
net.DNSConfigError
net.DNSError
net.PathError
net.SyscallError

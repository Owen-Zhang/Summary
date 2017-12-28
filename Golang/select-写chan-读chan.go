// Acquire 从池中获取一个资源
41 func (p *Pool) Acquire() (io.Closer, error) {
42 select {
43 // 检查是否有空闲的资源
44 case r, ok := <-p.resources:
45 log.Println("Acquire:", "Shared Resource")
46 if !ok {
47 return nil, ErrPoolClosed
48 }
49 return r, nil
50
51 // 因为没有空闲资源可用，所以提供一个新资源
52 default:
53 log.Println("Acquire:", "New Resource")
54 return p.factory()
55 }
56 } 

// Release 将一个使用后的资源放回池里
59 func (p *Pool) Release(r io.Closer) {
60 // 保证本操作和 Close 操作的安全
61 p.m.Lock()
defer p.m.Unlock()
63
64 // 如果池已经被关闭，销毁这个资源
65 if p.closed {
66 r.Close()
67 return
68 }
69
70 select {
71 // 试图将这个资源放入队列
72 case p.resources <- r:
73 log.Println("Release:", "In Queue")
74
75 // 如果队列已满，则关闭这个资源
76 default:
77 log.Println("Release:", "Closing")
78 r.Close()
79 }
80 }

// Close 会让资源池停止工作，并关闭所有现有的资源
83 func (p *Pool) Close() {
84 // 保证本操作与 Release 操作的安全
85 p.m.Lock()
86 defer p.m.Unlock()
87
88 // 如果 pool 已经被关闭，什么也不做
89 if p.closed {
90 return
91 }
92
93 // 将池关闭
94 p.closed = true
95
96 // 在清空通道里的资源之前，将通道关闭
97 // 如果不这样做，会发生死锁
98 close(p.resources)
99
100 // 关闭资源
101 for r := range p.resources {
102 r.Close()
103 }
104 } 

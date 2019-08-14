//错误做法
func ProcessChannelMessages(ctx context.Context, in <-chan string, idleCounter prometheus.Counter) {
    for {
        start := time.Now()
        select {
        case s, ok := <-in:
            if !ok {
                return
            }
            // handle `s`
        case <-time.After(5 * time.Minute):
            idleCounter.Inc()
        case <-ctx.Done():
            return
        }
    }
}

//正确写法
func ProcessChannelMessages(ctx context.Context, in <-chan string, idleCounter prometheus.Counter) {
    idleDuration := 5 * time.Minute
    idleDelay := time.NewTimer(idleDuration)
    defer idleDelay.Stop()
    for {
        idleDelay.Reset(idleDuration)
        select {
        case s, ok := <-in:
            if !ok {
                return
            }
            // handle `s`
        case <-idleDelay.C:
            idleCounter.Inc()
        case <-ctx.Done():
            return
        }
    }
}

func (c *Controller) Start() {

NEW_TICK_DURATION:
	c.Ticker = time.NewTicker(time.Second * 1)
	for {
		select {
		case <-c.WaitStoped:
			c.Ticker.Stop()
			return
		case <-c.Ticker.C:
			//c.Driver.Dispatch()
			c.Ticker.Stop()
			goto NEW_TICK_DURATION
		}
	}
}

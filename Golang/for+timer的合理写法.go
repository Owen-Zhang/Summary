package go

import (
	"time"
)

//不要在for 里面创建timer,以及time.After()
func (c *Client) HeartBeat() {
	beateDuration := time.Second * c.BeatingInternal
	beateTimer :=  time.NewTimer(beateDuration)
	defer beateTimer.Stop()
	for {
		beateTimer.Reset(beateDuration)
		select {
		case <-beateTimer.C:
			c.Release()
		case <-c.receiveHeart:
		}
	}
}
import os
import time
import board
import busio
import rospy
import argparse
import adafruit_mlx90640

from thermal_cam.msg import ThermalCameraMsg

class ThermalCamera():
    def __init__(self, rate : int = 2):
        rospy.init_node('thermal_cam')

        self.refresh_rate = adafruit_mlx90640.RefreshRate.REFRESH_2_HZ if rate==2 else\
                            adafruit_mlx90640.RefreshRate.REFRESH_4_HZ

        self.publish_rate = rospy.Rate(rate)

        self.i2c = busio.I2C(board.SCL, board.SDA,
                             frequency=800000) # TODO: replace magic numbers
        # TODO: graceful error handling

        self.mlx = adafruit_mlx90640.MLX90640(self.i2c)
        self.mlx.refresh_rate = self.refresh_rate

        framesize = ThermalCameraMsg.WIDTH * ThermalCameraMsg.HEIGHT
        self.dataframe = [0] * framesize

        self.thermals_publisher = rospy.Publisher("thermals", ThermalCameraMsg,
                                                  queue_size=1)

        self.run()

    def run(self):
        '''
        Function which fetches the data from the I2C connection, puts it into a message and publishes it
        '''
        frame_number = 0
        print('Running thermal cam node!')
        while not rospy.is_shutdown():
            frame_number += 1
            out_msg = ThermalCameraMsg()
            out_msg.header.stamp = rospy.Time.now()
            out_msg.header.frame_id = str(frame_number)
            try:
                self.mlx.getFrame(self.dataframe)
                out_msg.thermal_image = self.dataframe # this might need a deepcopy
            except ValueError:
                continue

            # the node will publish if it has or hasn't received data;
            # the subscriber should check for the validity of the thermal_image field
            self.thermals_publisher.publish(out_msg)

            # send out a msg and sleep
            self.publish_rate.sleep()

if __name__ == '__main__':

    parser = argparse.ArgumentParser(description="Module to send data from the thermal camera connected to a RPi4")

    parser.add_argument('--rate', '-r', type=int, default=2, choices=[2,4],
                        help="Rate at which the thermal data is published", nargs='?',
                        const=2)


    args = parser.parse_args()
    rate = args.rate


    try:
        thermal_camera = ThermalCamera(rate=rate)
    except rospy.ROSInterruptException:
        pass


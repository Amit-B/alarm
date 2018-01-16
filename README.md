# alarm

I was once (2012~) working with an organization called "Tzeva Adom". The name comes from the name of the emergency alarm of Israel.
As emergency events occured, the alarm sound could be heard in cities that were attacked, so citizen could go to the shelters.
The organization I was working with had a Facebook page of 100k subscribers, as well as website and many other platforms.

I wanted to expand their platforms with a desktop software with the feature of emergency alert immidiate report.
There was no any official API at the time. I could only count on the organization to report at their Facebook page, as the software was scanning RSS updates from it, popping up and playing a sound when there was a new update of alarm. As the organization published many news, I could only recognize when it's alarm based on specific keywords.

The software developed with C# and at the time was the ONLY known software that updates for emergency alarm.
Later some more desktop softwares and mobile apps came.

Note that the software is working with a web service for updates.
Since the real web service is inactive, and the Facebook API was changed many times, I know that the software can not be used today.

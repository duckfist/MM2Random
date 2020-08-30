# So you've decided to help improve MM2R2?

That's awesome! Thank you!

First, understand that as a group effort we sometimes need your contributions to look a certain way.
And sometimes we need to reach a consensus about how to do things. Both of those can things mean that
sometimes there is a lot of iteration before we can merge your changes. You should also know, we're
all volunteers and sometimes it takes a while before we can devote time to things. Gentle reminders
are encouraged if we're taking a while.

Generally speaking, we want to keep the `master` branch building and passing tests so that it is
easy to make new releases or for our beta testers to try out a new feature.

If you have a change you want us to consider, please open a pull request where we can review it
and discuss the design.

# Coding style

Where possible, try to match the conventions in the code that you're modying.

  * If you're contributing C# code, then please add tests and documentation. This isn't a strict rule,
  but if we all do our part it will help.

  * If you're contributing NES 6502 code, then please provide a pseudo-code description of that code.

# Design philosphy

We would like to provide the most general mm2 randomizer that we can. We believe that you have a
better idea of what is fun for you than we do. Therefore, we would rather add options with lots
of knobs and dials than providing OneTrueWay to do things.

That said, we do think the default should be similar to the vanilla experience. It should be
always possible under default settings to beat any of the 8 robo stages with just buster, no
items, and no zips. Similar logic applies to the wily stages but there we understand some areas
in vanilla require non-buster strats. So in those parts of the game the default randomization
shouldn't add new places that require non-buster strats.

# Discussion

As of writing this (Aug 30, 2020) we don't have a code of conduct yet (coming soon). Just know that
we do require you to be respectful of other people. Harassement will not be tolerated.

Please join our [Discord server](https://discord.gg/CmRjkSe) to discuss things. You're welcome to develop
new cool features of the randomizer before submitting a pull request, but please understand that
we may want you to change things. Some people find it easier to start that negotiation before writing
any code. We're open to either so just use your best judgement.

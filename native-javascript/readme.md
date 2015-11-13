# How to run

This project has been setup to be hosted with the zero-configuration `http-server` npm module.

Install: `npm install -g http-server`

Then start the server.

Run: `npm start`

Then connect your browser to <http://localhost:8080>

# Design Choices

The primary constraint on this task was the timescales. All software operates within a triangle of _Quality_ x _Time_ x _Features_.  Any project may fix two out of those three points, but is impossible to fix them all.  Given that the _Quality_ should never be sacrificed and _Time_ was the constraint of this demo it was important that I understand and prioritise the _Features_ to be implemented.

What were the choices that were made, and what is the justification for them?

1. It needed a quick and simple server to host the pages and the data.  I selected the `npm` module http-server for this, but you should be able to host it in any environment.  I don't believe this violated the requirement to not use any frameworks or libraries, as this was just the server and can be swapped with IIS/Apache/etc. easily.
2. TypeScript was put forward as a language choice, but prototype javascript was also available.  While I have read about TypeScript and attended a few talks on the topic I've not cut real code with it, so I wanted to eliminate this as a risk.  You should see that I've demonstrated a few things within the language, such as "use strict", callbacks, OO prototype.
3. There was no time for browser support, and given that the target audience are technical I went for Class A browser support only.  Even then the software was only run through Chrome on a MacBook Pro.  Under enterprise conditions this would obviously not be the case and understanding our target demographic would be key to making this choice.  This was part of the reason I used `XMLHttpRequest` to do the async data loading.
4. The application needed to look clean with minimal effort, so a little time was taken up front to make sure that it was presentable.  Roboto font and some colours lifted from Google's excellent Material Design.  The logic here is that given the scope of the problem a good UX/UI was not what was being delivered, but a bad UX/UI would detract from the professionalism of the project.  I've seen good technology let down by developers who couldn't present it well.
5. There is no unit testing.  This goes against my normal approach, but at this point I felt I would have been going against the requirement to not use frameworks and it would have taken too long to implement enough of a framework to satisfy the requirements.  But, the implementation has been separated out into different concerns which means that I could test it if required.
6. As with unit testing, there was no linter used, as I believed this would violate the no frameworks advice.  Therefore all the content is hand-rolled and checked by eye.
7. I wanted to keep the HTML and the TickerEngine logic separated, hence why I used the `{{xx}}` style substitutions.  Under normal circumstances this would be extracted into a framework or library to help me attach my ViewModel and View as this would almost certainly be re-used across projects.  As it was this code sits inside a `<script/>` tag at the bottom of the HTML page.
8. Within the TickerEngine I would normally have looked to implement Promises to reduce the nested callbacks and improve readability a little, but I decided that I should be using native js implementations.
9. I would normally have refactored out to have a single common namespace for the javascript functions because there is a good chance that I could find conflicts on `util` and `ticker`.  But given I had complete control and knew the scope of this problem, this wasn't the case.
10. Finally I implemented the UI flare as being a flipping animation.  I found the flipper appealing, and the animation code was interesting as it required me to chain events together so that I could get the "animate out => change value => animate in" behaviour working properly.

I hope this is a good demonstration of my skills in as lean a javascript environment as I could build in the available time, and I hope my explanations help you understand how I reached my conclusions.

Regards,
Chris

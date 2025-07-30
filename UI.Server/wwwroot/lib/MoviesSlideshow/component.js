import Glide from '@glidejs/glide'
import "@glidejs/glide/dist/css/glide.core.min.css";

let glideInstance = null;
export function renderComponent(selector) 
{
    const GlideTouchSupport = (function () {
        const events = {
            slideClick: e => {
                const slide = e.currentTarget;
                if (slide._slideClickTimeout)
                    clearTimeout(slide._slideClickTimeout);
                
                const shown = slide.classList.contains('glide__slide_show-overlay');
                if (!shown) {
                    e.preventDefault();
                    slide.classList.add('glide__slide_show-overlay');
                    // timeout to hide the overlay 
                    slide._slideClickTimeout = setTimeout(() => slide.classList.remove('glide__slide_show-overlay'), 7000);
                }
            }
        }
        
        return {
            build: function () {
                // only on touch-supporting devices
                if (!('ontouchstart' in window))
                    return;
                
                let slideNodes = document.querySelectorAll(selector + ' .glide__slide')
                slideNodes.forEach(slide => {
                    slide.addEventListener('click', events.slideClick);
                });
                document.addEventListener('touchstart', ev => {
                    // don't do anything if it was a click on the slide
                    const closestSlideNode = ev.target.closest('.glide__slide');
                    if (closestSlideNode) 
                        return;
                    
                    // otherwise, remove all active overlays from all slides
                    slideNodes.forEach(slide => {
                        slide.classList.remove('glide__slide_show-overlay');
                        if (slide._slideClickTimeout) 
                            clearTimeout(slide._slideClickTimeout);
                    });
                });
            },
            destroy: function (){
                let slideNodes = document.querySelectorAll(selector + ' .glide__slide')
                slideNodes.forEach(slide => {
                    slide.removeEventListener('click', events.slideClick);
                });
                document.removeEventListener('touchstart', events.slideClick);
            }
        }
    })();
    
    
    if (glideInstance) {
        try {
            glideInstance.destroy();
            GlideTouchSupport.destroy();
            
        } catch (error) {}
    }
    
    glideInstance = new Glide(selector, {
        type: 'carousel',
        perView: 3,
        focusAt: 'center',
        gap: 24,
        autoplay: 5000,
        breakpoints: {
            4000: {
                perView: 15
            },
            2500: {
                perView: 11
            },
            2000: {
                perView: 7
            },
            1500: {
                perView: 5
            },
            1200: {
                perView: 3
            },
            750: {
                perView: 2
            },
            500: {
                perView: 1
            }
        }
    });
    
    glideInstance.on('mount.after', function() {
        GlideTouchSupport.build();
    });

    glideInstance.mount();
}
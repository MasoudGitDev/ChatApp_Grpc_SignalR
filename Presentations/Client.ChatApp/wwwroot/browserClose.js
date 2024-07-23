function AddTabVisibilityListener(dotnetObjectRefrence) {
    document.addEventListener("visibilitychange", () => {
        document.visibilityState === "visible" ?
            dotnetObjectRefrence.invokeMethodAsync('OnTabVisibilityChanged', true) :
            dotnetObjectRefrence.invokeMethodAsync('OnTabVisibilityChanged', false);     
    });
}

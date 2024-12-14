import {environmentAPI} from "~/utils/environments/environmentAPI.js";
import {apiPaths} from "~/utils/paths/apiPaths.js";

export const getUrlForDestinations = (tripId) => {
    const baseUrl = environmentAPI.baseUrl;
    return `${baseUrl}/${apiPaths.trips}/${tripId}/${apiPaths.destinations}`;
};

export const getUrlForActivities = (tripId, activityId) => {
    const baseUrl = environmentAPI.baseUrl;
    return `${baseUrl}/${apiPaths.trips}/${tripId}/${apiPaths.destinations}/${activityId}/${apiPaths.activities}`;
};

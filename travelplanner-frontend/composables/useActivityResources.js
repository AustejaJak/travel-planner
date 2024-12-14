import {requestOptionsHelper} from "~/utils/requestOptionsHelper.js";
import {getUrlForActivities} from "~/utils/paths/getUrl.js";
import {Activity} from "~/model/activity.js";

export const useActivityResources = (tripId, destinationId) => {
    const urlActivity = getUrlForActivities(tripId, destinationId);

    const getActivity = async () => {
        try {
            const response = await fetch(urlActivity, requestOptionsHelper.get);
            const data = await response.json();

            return data.map(item => new Activity(item));
        } catch (error) {
            console.error('Error during getting trips', error);
            throw error;
        }
    };


    const postActivity = async (credentials) => {
        try {
            const response = await fetch(urlActivity, requestOptionsHelper.post(credentials));
            if (response.ok) {
                return {status: response.status, message: 'Trips successfully created'};
            } else {
                return {status: response.status, message: 'Could not create a trip.'};
            }
        } catch(error) {
            console.error('Error during creating a trip', error);
            throw error;
        }
    }

    const putActivity = async (credentials) => {
        try {
            const response = await fetch(urlActivity, requestOptionsHelper.put(credentials));
            if (response.ok) {
                return {status: response.status, message: 'Trip successfully updated'};
            } else {
                return {status: response.status, message: 'Could not updated a trip.'};
            }
        } catch(error) {
            console.error('Error during updating a trip', error);
            throw error;
        }
    }

    const deleteActivity = async () => {
        try {
            const response = await fetch(urlActivity, requestOptionsHelper.del);
            if (response.ok) {
                const data = response.json();
                console.log(data);
            } else {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Could not delete trip.');
            }
        } catch(error) {
            console.error('Error during deleting a trip', error);
            throw error;
        }
    }

    return{
        getActivity,
        postActivity,
        putActivity,
        deleteActivity
    };

}

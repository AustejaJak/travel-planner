import {requestOptionsHelper} from "~/utils/requestOptionsHelper.js";
import {getUrlForDestinations} from "~/utils/paths/getUrl.js";
import {Destination} from "~/model/destination.js";

export const useDestinationResources = (tripId) => {
    const urlDestination = getUrlForDestinations(tripId);

    const getDestination = async () => {
        try {
            const response = await fetch(urlDestination, requestOptionsHelper.get);
            const data = await response.json();

            return data.map(item => new Destination(item));
        } catch (error) {
            console.error('Error during getting trips', error);
            throw error;
        }
    };


    const postDestination = async (credentials) => {
        try {
            const response = await fetch(urlDestination, requestOptionsHelper.post(credentials));
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

    const putDestination = async (credentials) => {
        try {
            const response = await fetch(urlDestination, requestOptionsHelper.put(credentials));
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

    const deleteDestination= async () => {
        try {
            const response = await fetch(urlDestination, requestOptionsHelper.del);
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
        getDestination,
        postDestination,
        putDestination,
        deleteDestination
    };

}

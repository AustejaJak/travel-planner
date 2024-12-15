import {environmentAPI} from "~/utils/environments/environmentAPI.js";
import {apiPaths} from "~/utils/paths/apiPaths.js";
import {requestOptionsHelper} from "~/utils/requestOptionsHelper.js";
import {Trip} from "~/model/trip";

export const useTripResources = () => {
    const baseUrl = environmentAPI.baseUrl;

    const urlTrip = `${baseUrl}/${apiPaths.trips}?pageNumber=1&pageSize=5`;

    const getTrip = async () => {
        try {
            const response = await fetch(urlTrip, requestOptionsHelper.get);
            const data = await response.json();

            return data.map(item => new Trip(item));
        } catch (error) {
            console.error('Error during getting trips', error);
            throw error;
        }
    };

    const getTripByUserId = async (userId) => {
        try {
            const urlTripById = `${baseUrl}/${apiPaths.trips}/user/${userId}?pageNumber=1&pageSize=5`;
            const response = await fetch(urlTripById, requestOptionsHelper.get);
            const data = await response.json();

            return data.map(item => new Trip(item));
        } catch (error) {
            console.error('Error during getting trips', error);
            throw error;
        }
    };


    const postTrip = async (payload) => {
        try {
            const response = await fetch(urlTrip, requestOptionsHelper.post(payload));
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

    const putTrip = async (credentials, tripId) => {
        try {
            const urlTripWithId = `${baseUrl}/${apiPaths.trips}/${tripId}`
            const response = await fetch(urlTripWithId, requestOptionsHelper.put(credentials));
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

    const deleteTrip = async (tripId) => {
        try {
            const urlTripWithId = `${baseUrl}/${apiPaths.trips}/${tripId}`
            const response = await fetch(urlTripWithId, requestOptionsHelper.del);
            if (response.ok) {
                return {status: response.status, message: 'Trip succesfully deleted'};
            } else {
                return {status: response.status, message: 'Could not delete a trip.'};
            }
        } catch(error) {
            console.error('Error during deleting a trip', error);
            throw error;
        }
    }

    return{
        getTrip,
        getTripByUserId,
        postTrip,
        putTrip,
        deleteTrip
    };

}

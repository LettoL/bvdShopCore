import { createEvent, createStore } from 'effector';

export const setError = createEvent();
export const clearError = createEvent();

export const $errorStore = createStore([]);

$errorStore
	.on(setError, (state, error) => [...state, error])
	.on(clearError, (state) => [])